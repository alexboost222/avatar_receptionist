using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;

public class SimpleUnityIO : MonoBehaviour
{
    public TCPTransport transport;
    public PlayAudioFromPath playAudioFromPath;
    

    public TextMeshProUGUI text;
    public TMP_InputField inputField;

    /*private void OnEnable()
    {
        UniTask.Run(async () => await playAudioFromPath.PlayClip("/Users/stasiandr/GithubRepo/avatar_receptionist/test.raw"));
    }*/


    private void OnDisable()
    {
        transport.DeInit();
    }


    /*public void EnqueueMessage()
    {
        _mq.Enqueue(inputField.text);
        inputField.text = "";
    }

    private Queue<string> _mq;


    public async UniTask<JObject> HandleInput(JObject inp)
    {
        text.text = inp["msg"].Value<string>();
        
        await UniTask.WaitUntil(() => _mq.Count != 0);

        return new JObject
        {
            ["msg"] = _mq.Dequeue()
        };
    }*/
}
