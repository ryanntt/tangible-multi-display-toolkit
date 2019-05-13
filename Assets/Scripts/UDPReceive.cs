using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;

public class UDPReceive : MonoBehaviour
{

    // receiving Thread
    Thread receiveThread;

    // udpclient object
    UdpClient receiveClient;

    //TCP
    TcpClient mySocket;
    NetworkStream theStream;
    StreamWriter theWriter;
    Thread clientReceiveThread;

    // public
    public string IP = "192.168.0.107";
    public int receivePort; // define > init
    public int sendPort; // define > init

    // infos
    public string lastReceivedUDPPacket = "";
    public string allReceivedUDPPackets = ""; // clean up this from time to time!


    // start from shell
    private static void Main()
    {
        UDPReceive receiveObj = new UDPReceive();
        receiveObj.init();

        string text = "";
        do
        {
            text = Console.ReadLine();
        }
        while (!text.Equals("exit"));
    }
    // start from unity3d
    public void Start()
    {

        init();
    }


    // init
    private void init()
    {
        // Endpunkt definieren, von dem die Nachrichten gesendet werden.
        print("UDPSend.init()");

        // define port
        receivePort = 6454;
        sendPort = 6455;


        receiveClient = new UdpClient(receivePort);

        receiveThread = new Thread(
            new ThreadStart(() => ReceiveData(receiveClient)));
        receiveThread.IsBackground = true;
        receiveThread.Start();

        try
        {
            mySocket = new TcpClient(IP, 5566);
            theStream = mySocket.GetStream();
            theWriter = new StreamWriter(theStream);
        }
        catch (Exception e)
        {
            print("Socket error: " + e);
        }

        try
        {
            clientReceiveThread = new Thread(new ThreadStart(() => ListenForData(mySocket)));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
        }
        catch (Exception e)
        {
            print("On client connect exception " + e);
        }

    }


    private void ListenForData(TcpClient socket)
    {
        try
        {
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                // Get a stream object for reading              
                using (NetworkStream stream = socket.GetStream())
                {
                    int length;
                    // Read incomming stream into byte arrary.                  
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incommingData = new byte[length];
                        Array.Copy(bytes, 0, incommingData, 0, length);
                        // Convert byte array to string message.                        
                        string serverMessage = Encoding.ASCII.GetString(incommingData);
                        print("server message received as: " + serverMessage);

                        ContextManager ctxManager = GameObject.FindWithTag("Contexts").GetComponent<ContextManager>();
                        ctxManager.UpdateContexts(serverMessage);
                    }
                }
            }
        }
        catch (SocketException socketException)
        {
            print("Socket exception: " + socketException);
        }
    }

    public void writeSocket(string theLine)
    {
        String foo = theLine + "\r\n";
        theWriter.Write(foo);
        theWriter.Flush();
        print("writeTCP");
    }

    // receive thread
    private void ReceiveData(UdpClient client)
    {

        while (true)
        {

            try
            {
                // Bytes empfangen.
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] received = client.Receive(ref anyIP);

                print(anyIP.Address);

                // Bytes mit der UTF8-Kodierung in das Textformat kodieren.
                string text = Encoding.UTF8.GetString(received);

                if (text.Contains("Art-Net")) {

                    // Den abgerufenen Text anzeigen.
                    print(">> " + text);

                    int dmx = 18;
                    StringBuilder[] sb = new StringBuilder[21];
                    String[] str = new String[21];

                    for (int i = 0; i < 21; i++)
                    {
                        sb[i] = new StringBuilder();
                        sb[i].Append("#");
                        sb[i].Append(received[dmx++].ToString("X2"));
                        sb[i].Append(received[dmx++].ToString("X2"));
                        sb[i].Append(received[dmx++].ToString("X2"));
                        str[i] = sb[i].ToString();
                    }

                    //Output the colors received color values to the Car's LEDs

                    LEDControl led = GameObject.Find("LEDs").GetComponent<LEDControl>();
                    led.ChangeColor(str);

                    //writeSocket("test2");
                }
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

    private string ExtractString(byte[] packet, int start, int length)
    {
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < packet.Length; i++)
        {
            sb.Append((char)packet[i]);
        }
        return sb.ToString();
    }


    private void OnDestroy()
    {
        // make sure to clean up sockets on exit
        receiveClient.Client.Close();
        receiveClient.Close();
    }
}
