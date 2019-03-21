using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class UDPReceive : MonoBehaviour
{
    UdpClient udpClient;
    IPEndPoint RemoteIpEndPoint;

    //port exposed for unity
    [SerializeField] int EndPointPort = 6454;

    // Use this for initialization
    void Start()
    {
        RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, EndPointPort);

        udpClient = new UdpClient();
        udpClient.Client.Bind(RemoteIpEndPoint);

        PrepareForUDP();
    }


    //init an asynchronous callback to be called on reciept of a udp packet
    void PrepareForUDP()
    {
        AsyncCallback callback = new AsyncCallback(UDPMsgRecievedCallback);
        udpClient.BeginReceive(callback, null);
    }

    /// <summary>
    /// Function designed to be called when a UDP message is recieved
    /// if we wanted to listen for the next we might put another 
    /// 'BeginReceive()' method at the end of this method.
    /// </summary>
    /// <param name="result">Result.</param>
    private void UDPMsgRecievedCallback(IAsyncResult result)
    {
        byte[] received = udpClient.EndReceive(result, ref RemoteIpEndPoint);

        int dmx = 18;
        StringBuilder[] sb = new StringBuilder[21];
        String[] str = new String[21];

        for (int i = 0; i < 21; i++) {
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

        PrepareForUDP();
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
        udpClient.Client.Close();
        udpClient.Close();
    }

}
