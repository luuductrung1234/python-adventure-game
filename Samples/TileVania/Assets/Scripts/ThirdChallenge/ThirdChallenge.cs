using System;
using InGameCodeEditor;
using TMPro;
using UnityEngine;

public class ThirdChallenge : MonoBehaviour
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