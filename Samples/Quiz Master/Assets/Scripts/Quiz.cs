using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Quiz : MonoBehaviour
{
	[Header("Questions")]
	[SerializeField] TextMeshProUGUI questionText;
	[SerializeField] List<QuestionSO> questions = new();

	[Header("Answers")]
	[SerializeField] GameObject[] answerButtons;
	[SerializeField] Sprite defaultAnswerSprite;
	[SerializeField] Sprite correctAnswerSprite;

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

	public Action onQuizComplete;

	void Start()
	{
		progressBar.maxValue = questions.Count;
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

	private void GetNextQuestion()
	{
		if (questions == null || questions.Count == 0)
		{
			onQuizComplete?.Invoke();
			return;
		}

		SetButtonState(BTN_ENABLE);
		SetDefaultButtonSprite();
		var questionIndex = UnityEngine.Random.Range(0, questions.Count);
		currentQuestion = questions[questionIndex];
		questions.RemoveAt(questionIndex);
		DisplayQuestion();
		scoreKeeper.IncreaseQuestionSeen();
		progressBar.value++;
	}

	private void DisplayQuestion()
	{
		questionText.text = currentQuestion.GetQuestion();
		questionText.color = Color.white;

		var answers = currentQuestion.GetAnswers();
		for (int i = 0; i < answers.Length; i++)
		{
			var answer = answers[i];
			var buttonText = answerButtons[i].GetComponentInChildren<TextMeshProUGUI>();
			buttonText.text = answer;
		}
		isAnswered = false;
	}

	private void DisplayAnswer(int index)
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

	private void SetButtonState(bool state)
	{
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
}
