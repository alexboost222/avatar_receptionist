using System;
using UnityEngine;
using UnityEngine.UI;

public class DialogView : MonoBehaviour
{
    public event Action FirstButtonClicked;
    public event Action SecondButtonClicked;
    public event Action ThirdButtonClicked;
    public event Action ForthButtonClicked;
    public event Action StartButtonClicked;
    public event Action RestartButtonClicked;
    
    [SerializeField] private Text header;
    
    [SerializeField] private Text firstButtonText;
    [SerializeField] private Text secondButtonText;
    [SerializeField] private Text thirdButtonText;
    [SerializeField] private Text forthButtonText;

    [SerializeField] private GameObject notStartedView;
    [SerializeField] private GameObject inProgressView;
    [SerializeField] private GameObject finishedView;

    [SerializeField] private string notStartedHeaderText;
    [SerializeField] private string finishedHeaderText;

    public string Header
    {
        set => header.text = value;
    }

    public string FirstButtonText
    {
        set => firstButtonText.text = value;
    }

    public string SecondButtonText
    {
        set => secondButtonText.text = value;
    }

    public string ThirdButtonText
    {
        set => thirdButtonText.text = value;
    }

    public string ForthButtonText
    {
        set => forthButtonText.text = value;
    }

    public void FirstButtonClickedInvoker() => FirstButtonClicked?.Invoke();

    public void SecondButtonClickedInvoker() => SecondButtonClicked?.Invoke();

    public void ThirdButtonClickedInvoker() => ThirdButtonClicked?.Invoke();

    public void ForthButtonClickedInvoker() => ForthButtonClicked?.Invoke();

    public void StartButtonClickedInvoker() => StartButtonClicked?.Invoke();
    public void RestartButtonClickedInvoker() => RestartButtonClicked?.Invoke();

    public void DisplayNotStartedView()
    {
        Header = notStartedHeaderText;
        notStartedView.SetActive(true);
        inProgressView.SetActive(false);
        finishedView.SetActive(false);
    }

    public void DisplayInProgressView()
    {
        notStartedView.SetActive(false);
        inProgressView.SetActive(true);
        finishedView.SetActive(false);
    }

    public void DisplayFinishedView()
    {
        Header = finishedHeaderText;
        notStartedView.SetActive(false);
        inProgressView.SetActive(false);
        finishedView.SetActive(true);
    }
}