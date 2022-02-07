using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

[DisallowMultipleComponent]
public class DialogView : MonoBehaviour
{
    [SerializeField] private TMP_InputField input;
    [SerializeField] private TMP_Text output;

    private bool _inputSubmitted;
    private bool _inputSubmitionWaiting;

    public string Output
    {
        set => output.text = value;
    }

    public async UniTask<string> GetSubmittedInput()
    {
        _inputSubmitionWaiting = true;

        while (!_inputSubmitted)
        {
            await UniTask.DelayFrame(10);
        }

        _inputSubmitionWaiting = false;
        _inputSubmitted = false;

        return input.text;
    }

    private void Start()
    {
        input.onSubmit.AddListener(OnInputSubmit);
    }

    private void OnDestroy()
    {
        input.onSubmit.RemoveListener(OnInputSubmit);
    }

    private void OnInputSubmit(string _)
    {
        if (!_inputSubmitionWaiting) return;

        _inputSubmitted = true;
    }
}