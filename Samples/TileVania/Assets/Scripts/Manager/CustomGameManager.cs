/***************************************************************
*Title: n/a
*Author: Luu Duc Trung
*Date: n/a
*Availability: n/a
*Code version: n/a
****************************************************************/

using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CustomGameManager : MonoBehaviour
{
	[Header("General")]
	[SerializeField] TextMeshProUGUI title;

	[Header("Challenges")]
	[SerializeField] Quiz quiz;
	[SerializeField] FirstChallenge firstChallenge;
	[SerializeField] SecondChallenge secondChallenge;
	[SerializeField] ThirdChallenge thirdChallenge;

	[Header("End")]
	[SerializeField] EndScreen endScreen;

	public Action onAnswered;
	public Action onReset;

	void Start()
	{
		Reset();
		switch (SceneData.challengeMode)
		{
			case ChallengeMode.FirstChallenge:
				title.text = "CHALLENGE";
				SetupFirstChallenge();
				break;
			case ChallengeMode.SecondChallenge:
				title.text = "CHALLENGE";
				SetupSecondChallenge();
				break;
			case ChallengeMode.ThirdChallenge:
				title.text = "CHALLENGE";
				SetupThirdChallenge();
				break;
			default:
				title.text = "QUICK QUIZ";
				SetupQuiz();
				break;
		}
	}

	public void OnRetryQuiz()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void OnResumeMap()
	{
		if (SceneData.currentHealth == 0)
			SceneData.currentHealth = 1;
		SceneManager.LoadScene(SceneData.currentScene.ToString());
	}

	public void OnReset()
	{
		if (onReset == null)
		{
			Debug.LogError("OnReset hook must be configured for GameManager!");
			return;
		}
		this.onReset();
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
		Reset();
		endScreen.gameObject.SetActive(false);
		quiz.gameObject.SetActive(true);
		onAnswered = quiz.OnAnswered;
		quiz.onQuizComplete = (int score) =>
		{
			endScreen.gameObject.SetActive(true);
			endScreen.SetScoreText($"Congratulation!\nYou scored {score}%");
			quiz.gameObject.SetActive(false);
			int healthGain = (int)Math.Round(SceneData.maxHealth * score / 100.0);
			SceneData.currentHealth = healthGain;
			Debug.Log($"Finished the quiz, gain {healthGain}");
		};
	}

	public void SetupFirstChallenge()
	{
		Reset();
		endScreen.gameObject.SetActive(false);
		firstChallenge.gameObject.SetActive(true);
		onAnswered = firstChallenge.OnAnswered;
		onReset = firstChallenge.OnReset;
		firstChallenge.onChallengeSolved = () =>
		{
			endScreen.gameObject.SetActive(true);
			endScreen.SetScoreText("Congratulation! The first challenge has been solved.\nNew map unlocked!");
			firstChallenge.gameObject.SetActive(false);
		};
	}

	public void SetupSecondChallenge()
	{
		Reset();
		endScreen.gameObject.SetActive(false);
		secondChallenge.gameObject.SetActive(true);
		onAnswered = secondChallenge.OnAnswered;
		onReset = secondChallenge.OnReset;
		secondChallenge.onChallengeSolved = () =>
		{
			endScreen.gameObject.SetActive(true);
			endScreen.SetScoreText("Congratulation! The second challenge has been solved.\nNew map unlocked!");
			secondChallenge.gameObject.SetActive(false);
		};
	}

	public void SetupThirdChallenge()
	{
		Reset();
		endScreen.gameObject.SetActive(false);
		thirdChallenge.gameObject.SetActive(true);
		onAnswered = thirdChallenge.OnAnswered;
		onReset = thirdChallenge.OnReset;
		thirdChallenge.onChallengeSolved = () =>
		{
			endScreen.gameObject.SetActive(true);
			endScreen.SetScoreText("Congratulation! The third challenge has been solved.\nNew map unlocked!");
			thirdChallenge.gameObject.SetActive(false);
		};
	}

	private void Reset()
	{
		endScreen.gameObject.SetActive(false);
		firstChallenge.gameObject.SetActive(false);
		secondChallenge.gameObject.SetActive(false);
		thirdChallenge.gameObject.SetActive(false);
	}
}
