using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;
using static System.Text.Encoding;

[CreateAssetMenu]
public class TCPTransport : ScriptableObject
{
    private const int BufferLength = 1024 * 2;
    
    public string ip;
    public int port;

    private CancellationTokenSource _cts;
    
    public void Init(Func<JObject, UniTask<JObject>> handler)
    {
        _cts = new CancellationTokenSource();

        UniTask.Run(async () => await StartServer(handler), cancellationToken: _cts.Token);
    }


    private async UniTask StartServer(Func<JObject, UniTask<JObject>> handler)
    {
        await UniTask.SwitchToMainThread();

        TcpListener server=null;
        try
        {
            server = new TcpListener(IPAddress.Parse(ip), port);

            server.Start();

            byte[] bytes = new byte[BufferLength];
            
            while(Application.isPlaying)
            {
                Debug.Log("Waiting for a connection... ");

                var client = await server.AcceptTcpClientAsync();
                
                var stream = client.GetStream();
                int i;
                
                while((i = await stream.ReadAsync(bytes, 0, bytes.Length, _cts.Token)) != 0)
                {
                    Decode(ref bytes, i, out var json);
                    
                    Encode(out var msg, await handler(json));
                    stream.Write(msg, 0, msg.Length);
                }

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


    private static void Decode(ref byte[] data, int len, out JObject json)
    {
        json = JObject.Parse(UTF8.GetString(data, 0, len));
        
        Debug.Log(json.ToString());
    }

    private void Encode(out byte[] msg, in JObject json)
    {
        Debug.Log(json.ToString());
        
        msg = UTF8.GetBytes(json.ToString());
    }
    
    
    
    

    public void DeInit()
    {
        _cts.Cancel();
    }
        
}
