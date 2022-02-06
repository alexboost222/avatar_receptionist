using System;
using System.Net;
using System.Net.Sockets;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;

[CreateAssetMenu]
public class TCPTransport : ScriptableObject
{
    public async UniTask Init(Func<JObject, UniTask<JObject>> handler)
    {
        await UniTask.SwitchToMainThread();

        TcpListener server=null;
        try
        {
            // Set the TcpListener on port 13000.
            int port = 8085;
            var localAddr = IPAddress.Parse("127.0.0.1");

            // TcpListener server = new TcpListener(port);
            server = new TcpListener(localAddr, port);

            // Start listening for client requests.
            server.Start();

            // Buffer for reading data
            byte[] bytes = new byte[1024];

            // Enter the listening loop.
            while(Application.isPlaying)
            {
                Debug.Log("Waiting for a connection... ");

                // Perform a blocking call to accept requests.
                // You could also use server.AcceptSocket() here.
                TcpClient client = await server.AcceptTcpClientAsync();
                
                Debug.Log("Connected!");

                // Get a stream object for reading and writing
                NetworkStream stream = client.GetStream();

                int i;

                // Loop to receive all the data sent by the client.
                while((i = await stream.ReadAsync(bytes, 0, bytes.Length))!=0)
                {
                    // Translate data bytes to a ASCII string.
                    var data = System.Text.Encoding.UTF8.GetString(bytes, 0, i);
                    
                    Debug.Log($"Received: {data}");

                    // Process the data sent by the client.
                    data = data.ToUpper();

                    byte[] msg = System.Text.Encoding.UTF8.GetBytes(data);

                    // Send back a response.
                    stream.Write(msg, 0, msg.Length);
                    
                    Debug.Log($"Sent: {data}");
                }

                // Shutdown and end connection
                client.Close();
            }
        }
        catch(SocketException e)
        {
            Debug.Log($"SocketException: {e}");
        }
        finally
        {
            // Stop listening for new clients.
            server?.Stop();
        }
    }
        
}
