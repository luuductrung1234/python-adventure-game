using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomGameManager : MonoBehaviour
{
	[Header("Challenges")]
	[SerializeField] Quiz quiz;
	[SerializeField] FirstChallenge firstChallenge;
	[SerializeField] SecondChallenge secondChallenge;
	[SerializeField] ThirdChallenge thirdChallenge;

	[Header("End")]
	[SerializeField] EndScreen endScreen;

	public Action onAnswered;

	void Start()
	{
		SetupQuiz();
	}

	public void OnReplayLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void OnAnswered()
	{
		if (onAnswered == null)
		{
			Debug.LogError("OnAnswer hook must be configured for GameManager!");
			return;
		}
		this.onAnswered();
	}

	public void SetupQuiz()
	{
		endScreen.gameObject.SetActive(false);
		quiz.gameObject.SetActive(true);
		onAnswered = quiz.OnAnswer;
		quiz.onQuizComplete = () =>
		{
			endScreen.gameObject.SetActive(true);
			quiz.gameObject.SetActive(false);
		};
	}
}
