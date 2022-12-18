using System;
using InGameCodeEditor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SecondChallenge : MonoBehaviour
{
	[Header("Questions")]
	[SerializeField] TextMeshProUGUI questionText;
	[SerializeField] TextMeshProUGUI warningText;
	[SerializeField] GameObject descriptionView;
	[SerializeField] GameObject resultView;
	[SerializeField] Button nextButton;

	[Header("Answer By Code")]
	[SerializeField]
	private CodeEditor codeEditor;

	public Action onChallengeSolved;

	private void Start()
	{
		codeEditor.Text = seedAnswer;
		nextButton.interactable = false;
		OnShowDescription();
	}

	public void OnReset()
	{
		codeEditor.Text = seedAnswer;
		nextButton.interactable = false;
	}

	public void OnAnswered()
	{
		// TODO: validate code + is the code solve the problem?
	}

	public void OnNext()
	{
		onChallengeSolved();
	}

	public void OnShowDescription()
	{
		descriptionView.SetActive(true);
		resultView.SetActive(false);
	}

	public void OnShowResult()
	{
		descriptionView.SetActive(false);
		resultView.SetActive(true);
	}

	private const string seedAnswer = @"# import sys
# import System
# you can import build-in modules (of Python or .NET Clr)

from_rod = 'A'
aux_rod = 'B'
to_rod = 'C'
disks_num = 3

# move all disks (3) from rod A to rod C

# answer start

def answer():
	# write your own code to solve the challenge here...
	steps = [
		{ 'disk': 1, 'from': from_rod, 'to': to_rod },
		{ 'disk': 2, 'from': from_rod, 'to': aux_rod },
		{ 'disk': 1, 'from': to_rod, 'to': aux_rod },
	]
	return steps

# answer end
";
}