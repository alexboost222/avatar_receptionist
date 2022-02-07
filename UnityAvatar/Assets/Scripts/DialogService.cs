using Cysharp.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class DialogService : MonoBehaviour
{
    [SerializeField] private TCPTransport transport;
    [SerializeField] private DialogView view;
    [SerializeField] private PlayAudioFromPath playAudioFromPath;

    private void Start()
    {
        transport.Init(Handler);
    }

    private void OnDestroy()
    {
        transport.DeInit();
    }

    private async UniTask<JObject> Handler(JObject input)
    {
        string msg = input["msg"].ToString();
        view.Output = msg;
        string avatarSpeechFilepath = input["avatar_speech_filepath"].ToString();
        await playAudioFromPath.PlayClip(avatarSpeechFilepath);
        string userMsg = await view.GetSubmittedInput();
        JObject response = new JObject
        {
            ["msg"] = userMsg
        };
        return response;
    }
}