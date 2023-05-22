using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.IO;
using System.Text;

public class DataImporter : MonoBehaviour
{
    TcpListener listener;
    String msg;
    public bool left, right, down, pause;
    private float toggleTimer;
 
    // Start is called before the first frame update
    void Start()
    {
        string hostName = Dns.GetHostName();
        IPAddress[] addresses = Dns.GetHostAddresses(hostName);
        IPAddress localAddress = Array.Find(addresses, a => a.AddressFamily == AddressFamily.InterNetwork);
        left = false;
        right = false;
        down = false;
        pause = false;
        toggleTimer = 0.0f;
        listener = new TcpListener(IPAddress.Parse(localAddress.ToString()),55001);
        listener.Start();
        print("is listening");
    }

    // Update is called once per frame
    void Update()
    {
        if (!listener.Pending())
        {
        }
        else
        {
            print("socket comes");
            TcpClient client = listener.AcceptTcpClient();
            NetworkStream ns = client.GetStream();
            StreamReader reader = new StreamReader(ns);
            msg = reader.ReadToEnd();
            print(msg);
        }
        //runs timer
        toggleTimer += Time.deltaTime;

        
        //usually should import the data from Matlab. Right now uses arrow keys
        //if(Input.GetKeyDown(KeyCode.D) && toggleTimer > 0.2f)
        if(msg == "right" && toggleTimer > 0.2f)
        {
            msg = "";
            right = true;
            left = false;
            down = false;
            pause = false;
            toggleTimer = 0.0f;
            Debug.Log("Rechts");
        }
        if(Input.GetKeyDown(KeyCode.D) && toggleTimer > 0.2f)
        {
            msg = "";
            right = true;
            left = false;
            down = false;
            pause = false;
            toggleTimer = 0.0f;
            Debug.Log("Rechts");
        }

        //if(Input.GetKeyDown(KeyCode.A) && toggleTimer > 0.2f)
        if(msg == "left" && toggleTimer > 0.2f)
        {
            msg = "";
            right = false;
            left = true;
            down = false;
            pause = false;
            toggleTimer = 0.0f;
            Debug.Log("LINKS");
        }

        if(Input.GetKeyDown(KeyCode.A) && toggleTimer > 0.2f)
        {
            msg = "";
            right = false;
            left = true;
            down = false;
            pause = false;
            toggleTimer = 0.0f;
            Debug.Log("LINKS");
        }
        //if(Input.GetKeyDown(KeyCode.S) && toggleTimer > 0.2f)
        if(msg == "drop" && toggleTimer > 0.2f)
        {
            msg = "";
            right = false;
            right = false;
            down = true;
            pause = false;
            toggleTimer = 0.0f;
        }


        if(Input.GetKeyDown(KeyCode.S) && toggleTimer > 0.2f)
        {
            msg = "";
            right = false;
            right = false;
            down = true;
            pause = false;
            toggleTimer = 0.0f;
        }

                //if(Input.GetKeyDown(KeyCode.S) && toggleTimer > 0.2f)
        if(msg == "menu" && toggleTimer > 0.2f)
        {
            msg = "";
            right = false;
            right = false;
            down = false;
            pause = true;
            toggleTimer = 0.0f;
        }

        if(Input.GetKeyDown(KeyCode.Escape) && toggleTimer > 0.2f)
        {
            msg = "";
            right = false;
            right = false;
            down = false;
            pause = true;
            toggleTimer = 0.0f;
        }

        //ensures that the variables are only set to true for max. 0.1s
        if(toggleTimer > 0.1f)
        {
            left = false;
            right = false;
            down = false;
            pause = false;
        }

    }
}
