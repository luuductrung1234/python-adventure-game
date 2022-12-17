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

	public void OnRetryQuiz()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void OnResumeMap()
	{
		if (SceneData.currentHealth == 0)
			SceneData.currentHealth = 1;
		SceneManager.LoadScene(SceneData.currentScene);
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
		quiz.onQuizComplete = (int score) =>
		{
			endScreen.gameObject.SetActive(true);
			quiz.gameObject.SetActive(false);
			int healthGain = (int)Math.Round(SceneData.maxHealth * score / 100.0);
			SceneData.currentHealth = healthGain;
			Debug.Log($"Finished the quiz, gain {healthGain}");
		};
	}
}
