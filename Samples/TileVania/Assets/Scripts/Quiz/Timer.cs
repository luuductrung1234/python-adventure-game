/***************************************************************
*Title: n/a
*Author: Luu Duc Trung
*Date: n/a
*Availability: n/a
*Code version: n/a
****************************************************************/

using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	[SerializeField] float timeToCompleteQuestion = 30f;
	[SerializeField] float timeToShowCorrectAnswer = 7f;
	[SerializeField] GameObject timerImage;
	[SerializeField] GameObject idleImage;
	public bool isAnsweringQuestion = false;
	public Action<bool> timeoutAction = null;
	private float timerValue;

	private void Start()
	{
		Debug.Log("Trigger Timer start");
		timerValue = 0f;
	}

	private void Update()
	{
		UpdateTimer();
		UpdateTimerImage();
	}

	public void CancelTimer()
	{
		timerValue = 0f;
	}

	private void UpdateTimerImage()
	{
		if (isAnsweringQuestion)
		{
			var image = timerImage.GetComponent<Image>();
			timerImage.SetActive(true);
			idleImage.SetActive(false);
			image.fillAmount = timerValue / GetTimeByStage();
		}
		else
		{
			var image = idleImage.GetComponent<Image>();
			timerImage.SetActive(false);
			idleImage.SetActive(true);
			image.fillAmount = timerValue / GetTimeByStage();
		}
	}

	private void UpdateTimer()
	{
		timerValue -= Time.deltaTime;
		if (timerValue <= 0)
		{
			isAnsweringQuestion = !isAnsweringQuestion;
			timerValue = GetTimeByStage();
			timeoutAction?.Invoke(isAnsweringQuestion);
		}
	}

	private float GetTimeByStage()
		=> isAnsweringQuestion ? timeToCompleteQuestion : timeToShowCorrectAnswer;
}
