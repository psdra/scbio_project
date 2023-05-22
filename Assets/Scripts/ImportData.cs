using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class ImportData : MonoBehaviour
{
    private UdpClient udpClient;
    private int port = 12345; // Puerto de recepci�n
    public string receivedData; // Datos recibidos

    // M�todo Start, se ejecuta al inicio del juego
    void Start()
    {
        // Inicializar el cliente UDP
        udpClient = new UdpClient(port);

        // Comenzar a recibir datos en un hilo separado
        StartCoroutine(ReceiveData());
    }

    // M�todo para recibir datos en un hilo separado
    public IEnumerator ReceiveData()
    {
        while (true)
        {
            // Esperar a recibir datos
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, port);
            byte[] receivedBytes = udpClient.Receive(ref remoteEndPoint);

            // Convertir los datos recibidos en una cadena de texto
            receivedData = Encoding.ASCII.GetString(receivedBytes);

            // Realizar acciones con los datos recibidos
            // ...

            yield return null;
        }
    }

    // M�todo Update, se ejecuta en cada fotograma del juego
    void Update()
    {
        // Realizar acciones continuas o actualizaciones basadas en los datos recibidos
        // ...
    }

    // M�todo OnDestroy, se ejecuta al finalizar el juego
    void OnDestroy()
    {
        // Cerrar el cliente UDP al finalizar
        udpClient.Close();
    }
}