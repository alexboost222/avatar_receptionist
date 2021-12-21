using System;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    [SerializeField] private DialogView dialogView;
    [SerializeField] private DialogModel dialogModel;

    private readonly string[] _questions = {"Test question 1"};
    private readonly string[][] _answers = {new[] {"lol", "kek", "meme", "kross"}};

    private void Awake()
    {
        dialogModel.DialogStateChanged += OnDialogStateChanged;
        dialogModel.Init(_questions, _answers);
    }

    private void OnDestroy() => dialogModel.DialogStateChanged -= OnDialogStateChanged;

    private void OnFirstButtonClicked() => dialogModel.FirstAnswer();

    private void OnSecondButtonClicked() => dialogModel.SecondAnswer();

    private void OnThirdButtonClicked() => dialogModel.ThirdAnswer();

    private void OnForthButtonClicked() => dialogModel.ForthAnswer();

    private void OnStartButtonClicked() => dialogModel.StartDialog();

    private void OnRestartButtonClicked() => dialogModel.RestartDialogAfterFinish();

    private void OnDialogStateChanged(DialogModel.DialogState state)
    {
        switch (state)
        {
            case DialogModel.DialogState.NotStarted:
                dialogView.DisplayNotStartedView();
                dialogView.FirstButtonClicked -= OnFirstButtonClicked;
                dialogView.SecondButtonClicked -= OnSecondButtonClicked;
                dialogView.ThirdButtonClicked -= OnThirdButtonClicked;
                dialogView.ForthButtonClicked -= OnForthButtonClicked;
                dialogView.StartButtonClicked += OnStartButtonClicked;
                dialogView.RestartButtonClicked -= OnRestartButtonClicked;
                break;
            case DialogModel.DialogState.InProgress:
                dialogModel.QuestionChanged += OnQuestionChanged;
                dialogView.DisplayInProgressView();
                dialogView.FirstButtonClicked += OnFirstButtonClicked;
                dialogView.SecondButtonClicked += OnSecondButtonClicked;
                dialogView.ThirdButtonClicked += OnThirdButtonClicked;
                dialogView.ForthButtonClicked += OnForthButtonClicked;
                dialogView.StartButtonClicked -= OnStartButtonClicked;
                dialogView.RestartButtonClicked -= OnRestartButtonClicked;
                break;
            case DialogModel.DialogState.Finished:
                dialogView.DisplayFinishedView();
                dialogView.FirstButtonClicked -= OnFirstButtonClicked;
                dialogView.SecondButtonClicked -= OnSecondButtonClicked;
                dialogView.ThirdButtonClicked -= OnThirdButtonClicked;
                dialogView.ForthButtonClicked -= OnForthButtonClicked;
                dialogView.StartButtonClicked -= OnStartButtonClicked;
                dialogView.RestartButtonClicked += OnRestartButtonClicked;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    private void OnQuestionChanged(string question, string[] answer)
    {
        dialogView.Header = question;

        if (answer.Length != 4)
            throw new InvalidOperationException($"Answers array length must be 4, but not {answer}");

        dialogView.FirstButtonText = answer[0];
        dialogView.SecondButtonText = answer[1];
        dialogView.ThirdButtonText = answer[2];
        dialogView.ForthButtonText = answer[3];
    }
}