using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class SimpleUnityIO : MonoBehaviour
{
    public TCPTransport transport;

    private void Start()
    {
        UniTask.Run(async () => await transport.Init(HandleInput));
    }

    public async UniTask<JObject> HandleInput(JObject str)
    {
        await UniTask.Delay(1000);

        return str;
    }
}
