using System;
using UnityEngine;

public class DialogModel : MonoBehaviour
{
    public event Action<DialogState> DialogStateChanged;
    public event Action<string , string[]> QuestionChanged; 
    
    public enum DialogState
    {
        NotStarted,
        InProgress,
        Finished
    }

    private DialogState _dialogState;

    private string[][] _answers;

    private string[] _questions;

    private int[] _answerNumbers;

    private int _questionNumber;

    public DialogState DialogStateProp
    {
        get => _dialogState;
        private set
        {
            _dialogState = value;
            DialogStateChanged?.Invoke(_dialogState);
        }
    }

    public void StartDialog()
    {
        if (DialogStateProp != DialogState.NotStarted) return;

        DialogStateProp = DialogState.InProgress;
        
        _answerNumbers = new int[_questions.Length];

        NextQuestion();
    }

    public void FirstAnswer()
    {
        if (DialogStateProp != DialogState.InProgress) return;

        _answerNumbers[_questionNumber++] = 0;

        NextQuestion();
    }

    public void SecondAnswer()
    {
        if (DialogStateProp != DialogState.InProgress) return;

        _answerNumbers[_questionNumber++] = 1;

        NextQuestion();
    }

    public void ThirdAnswer()
    {
        if (DialogStateProp != DialogState.InProgress) return;

        _answerNumbers[_questionNumber++] = 2;

        NextQuestion();
    }

    public void ForthAnswer()
    {
        if (DialogStateProp != DialogState.InProgress) return;

        _answerNumbers[_questionNumber++] = 3;

        NextQuestion();
    }

    public void RestartDialogAfterFinish()
    {
        if (DialogStateProp != DialogState.Finished) return;

        DialogStateProp = DialogState.NotStarted;
        _questionNumber = 0;
    }

    public void Init(string[] questions, string[][] answers)
    {
        if (questions.Length != answers.Length)
            throw new InvalidOperationException(
                $"Questions array length {questions.Length} must be equal to answers array length {answers.Length}");

        _questions = questions;
        _answers = answers;

        DialogStateProp = DialogState.NotStarted;
    }

    private void NextQuestion()
    {
        if (_questions.Length == _questionNumber) DialogStateProp = DialogState.Finished;
        else QuestionChanged?.Invoke(_questions[_questionNumber], _answers[_questionNumber]);
    }
}