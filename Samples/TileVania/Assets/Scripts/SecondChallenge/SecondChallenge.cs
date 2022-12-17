using System;
using InGameCodeEditor;
using TMPro;
using UnityEngine;

public class SecondChallenge : MonoBehaviour
{
	[Header("Questions")]
	[SerializeField] TextMeshProUGUI questionText;

	[Header("Answer By Code")]
	[SerializeField]
	private CodeEditor codeEditor;

	public Action onChallengeSolved;

	public void OnReset()
	{

	}

	public void OnAnswered()
	{

	}
}