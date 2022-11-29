using System;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
	[SerializeField] float timeToCompleteQuestion = 30f;
	[SerializeField] float timeToShowCorrectAnswer = 10f;
	[SerializeField] Image timerImage;
	public bool isAnsweringQuestion = false;
	public Action<bool> timeoutAction = null;
	private float timerValue;

	private void Start()
	{
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
		timerImage.fillAmount = timerValue / GetTimeByStage();
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
