using System;
using System.Collections.Generic;
using InGameCodeEditor;
using IronPython.Custom;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
	[Header("Questions")]
	[SerializeField] TextMeshProUGUI questionText;
	[SerializeField] List<QuestionSO> questions = new();

	[Header("Answer By Click")]
	[SerializeField] bool answerByClick = false;
	[SerializeField] GameObject[] answerButtons;
	[SerializeField] Sprite defaultAnswerSprite;
	[SerializeField] Sprite correctAnswerSprite;

	[Header("Answer By Code")]
	[SerializeField]
	private CodeEditor codeEditor;
	[SerializeField] GameObject answerView;
	[SerializeField] GameObject yourAnswerView;

	[Header("Timer")]
	[SerializeField] Timer timer;

	[Header("Score")]
	[SerializeField] ScoreKeeper scoreKeeper;

	[Header("ProgressBar")]
	[SerializeField] Slider progressBar;

	private QuestionSO currentQuestion;
	private bool isAnswered = false;
	private static readonly bool BTN_ENABLE = true;
	private static readonly bool BTN_DISABLE = true;
	private int maxQuestion = 3;

	public Action<int> onQuizComplete;

	void Start()
	{
		progressBar.maxValue = maxQuestion;
		progressBar.value = 0;

		timer.timeoutAction = (bool isAnsweringQuestion) =>
		{
			if (isAnsweringQuestion)
				GetNextQuestion();
			else if (!isAnswered)
				DisplayAnswer(-1);
		};
	}

	public void OnAnswerSelected(int index)
	{
		isAnswered = true;
		DisplayAnswer(index);
		timer.CancelTimer();
	}

	public void OnAnswered()
	{
		if (codeEditor == null || string.IsNullOrEmpty(codeEditor.Text))
			return;
		DisplayAnswer(-1);
	}

	private void GetNextQuestion()
	{
		if (questions == null || questions.Count == 0 || progressBar.value == maxQuestion)
		{
			onQuizComplete?.Invoke(scoreKeeper.CalculateScore());
			return;
		}

		// reset Quiz state
		SetButtonState(BTN_ENABLE);
		SetDefaultButtonSprite();
		ClearAllAnswerViews();

		// show next question
		var questionIndex = UnityEngine.Random.Range(0, questions.Count);
		currentQuestion = questions[questionIndex];
		questions.RemoveAt(questionIndex);
		DisplayQuestion();

		// update score
		scoreKeeper.IncreaseQuestionSeen();
		progressBar.value++;
	}

	private void DisplayQuestion()
	{
		questionText.text = currentQuestion.GetQuestion();

		if (answerByClick)
		{
			var answers = currentQuestion.GetAnswers();
			for (int i = 0; i < answers.Length; i++)
			{
				answerButtons[i].SetActive(true);
				var answer = answers[i];
				var buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
				buttonText.text = answer;
			}
			yourAnswerView.SetActive(false);
		}
		else
		{
			foreach (var answerButton in answerButtons)
			{
				answerButton.SetActive(false);
			}
			yourAnswerView.SetActive(true);
			codeEditor.Text = currentQuestion.GetSeedAnswer();
		}
		isAnswered = false;
	}

	private void DisplayAnswer(int index)
	{
		if (answerByClick)
		{
			Image buttonImage;
			if (index == currentQuestion.GetCorrectAnswerIndex())
			{
				questionText.text = "Correct!";
				questionText.color = Color.green;
				buttonImage = answerButtons[index].GetComponent<Image>();
				scoreKeeper.IncreaseCorrectAnswer();
			}
			else
			{
				questionText.text = "Wrong!";
				questionText.color = Color.red;
				buttonImage = answerButtons[currentQuestion.GetCorrectAnswerIndex()].GetComponent<Image>();
			}
			buttonImage.sprite = correctAnswerSprite;
			SetButtonState(BTN_DISABLE);
		}
		else
		{
			var answerText = answerView.GetComponentInChildren<TextMeshProUGUI>();
			var yourAnswerText = yourAnswerView.GetComponentInChildren<TextMeshProUGUI>();
			var expectedResult = currentQuestion.GetAnswers()[currentQuestion.GetCorrectAnswerIndex()];

			var myPython = new MyPython(Debug.Log);
			var isCorrect = myPython.QuickAssert(codeEditor.Text, "answer", out var output, expectedResult);

			if (output != null)
			{
				if (isCorrect)
				{
					answerView.SetActive(true);
					answerText.text = $"correct result: {expectedResult}";
					yourAnswerText.text = $"your answer is correct!";
					yourAnswerText.color = Color.green;
					scoreKeeper.IncreaseCorrectAnswer();
					isAnswered = true;
					timer.CancelTimer();
				}
				else
				{
					yourAnswerText.text = $"your answer is wrong!";
					yourAnswerText.color = Color.red;
				}
			}
			else
			{
				yourAnswerText.text = "cannot analyze your answer. Please follow the given answer structure!";
				yourAnswerText.color = Color.red;
			}
		}
	}

	private void SetButtonState(bool state)
	{
		if (!answerByClick)
			return;
		foreach (var answerButton in answerButtons)
		{
			var button = answerButton.GetComponent<Button>();
			button.interactable = state;
		}
	}

	private void SetDefaultButtonSprite()
	{
		foreach (var answerButton in answerButtons)
		{
			var buttonImage = answerButton.GetComponent<Image>();
			buttonImage.sprite = defaultAnswerSprite;
		}
	}

	private void ClearAllAnswerViews()
	{
		questionText.text = string.Empty;
		questionText.color = Color.white;

		var answerText = answerView.GetComponentInChildren<TextMeshProUGUI>();
		answerText.text = "correct result: ";
		answerView.SetActive(false);
		var yourAnswerText = yourAnswerView.GetComponentInChildren<TextMeshProUGUI>();
		yourAnswerText.text = "your result: ";
		yourAnswerText.color = Color.white;
	}
}
