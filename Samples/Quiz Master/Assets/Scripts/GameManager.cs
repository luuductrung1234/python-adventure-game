using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	[SerializeField] Quiz quiz;
	[SerializeField] EndScreen endScreen;

	void Start()
	{
		endScreen.gameObject.SetActive(false);
		quiz.gameObject.SetActive(true);
		quiz.onQuizComplete = () =>
		{
			endScreen.gameObject.SetActive(true);
			quiz.gameObject.SetActive(false);
		};
	}

	public void OnReplayLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}
}
