using UnityEngine;
using System.Collections;

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceive : MonoBehaviour
{

    // receiving Thread
    Thread receiveThread;
    Thread sendThread;

    // udpclient object
    UdpClient receiveClient;
    UdpClient sendClient;

    // send data endpoint
    IPEndPoint remoteEndPoint;


    // public
    public string IP = "192.168.0.103";
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

        // status
        //print("Sending to 127.0.0.1 : " + port);
        //print("Test-Sending to this Port: nc -u 127.0.0.1  " + port + "");


        //remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);

        // ----------------------------
        // Abhören
        // ----------------------------
        // Lokalen Endpunkt definieren (wo Nachrichten empfangen werden).
        // Einen neuen Thread für den Empfang eingehender Nachrichten erstellen.
        receiveClient = new UdpClient(receivePort);

        sendClient = new UdpClient(sendPort);


        receiveThread = new Thread(
            new ThreadStart(() => ReceiveData(receiveClient)));
        receiveThread.IsBackground = true;
        receiveThread.Start();

        sendThread = new Thread(
            new ThreadStart(() => SendData(sendClient)));
        sendThread.IsBackground = true;
        sendThread.Start();

        /*while (true)
        {
            string text = "test to ipad";
            byte[] data = Encoding.UTF8.GetBytes(text);
            client.Send(data, data.Length, "192.168.0.103", 6454);
        }*/

    }

    public void SendData(UdpClient sendClient)
    {
        print("Send to iPad");

        while (true)
        {
            try
            {
                print("Send to iPad");
                string text = "test to ipad from second screen";
                byte[] data = Encoding.UTF8.GetBytes(text);
                sendClient.Send(data, data.Length, "192.168.0.103", sendPort);
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
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

                /*for (int i=0; i<str.Length; i++)
                {
                    Debug.Log("LEDs: " + str[i]);
                }*/

                //Output the colors received color values to the Car's LEDs
                LEDControl led = GetComponent<LEDControl>();
                led.ChangeColor(str);

                //send();
                //string t = "test to ipad";
                //byte[] data = Encoding.UTF8.GetBytes(t);
                //client.Send(data, data.Length, "192.168.0.103", 6454);

            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

    /*public void SendData(UdpClient client, IPEndPoint remoteEndPoint)
    {
        try
        {
            string text;
            do
            {
                text = "test to ipad";

                // Den Text zum Remote-Client senden.
                if (text != "")
                {
                    remoteEndPoint = new IPEndPoint(IPAddress.Parse(IP), port);

                    // Daten mit der UTF8-Kodierung in das Binärformat kodieren.
                    byte[] data = Encoding.UTF8.GetBytes(text);

                    // Den Text zum Remote-Client senden.
                    client.Send(data, data.Length, remoteEndPoint);

                    print(">> " + text);
                }
            } while (text != "");
        }
        catch (Exception err)
        {
            print(err.ToString());
        }
    }*/


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