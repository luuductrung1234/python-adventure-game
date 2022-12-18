using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InGameCodeEditor;
using IronPython.Custom;
using IronPython.Runtime;
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

	[Header("InGame Object")]
	[SerializeField] GameObject robA;
	[SerializeField] GameObject robB;
	[SerializeField] GameObject robC;
	[SerializeField]
	private List<GameObject> disksOnRobA;
	[SerializeField]
	private List<GameObject> disksOnRobB;
	[SerializeField]
	private List<GameObject> disksOnRobC;
	private IronPython.Runtime.List steps;

	public Action onChallengeSolved;

	private void Start()
	{
		codeEditor.Text = seedAnswer;
		warningText.text = string.Empty;
		nextButton.interactable = false;
		OnShowDescription();
		ResetRobs();
	}

	public void OnReset()
	{
		codeEditor.Text = seedAnswer;
		warningText.text = string.Empty;
		nextButton.interactable = false;
		ResetRobs();
	}

	public void OnAnswered()
	{
		var sourceCode = codeEditor.Text;
		var myPython = new MyPython(Debug.Log);
		warningText.text = string.Empty;
		var scope = myPython.RunScript(sourceCode);
		dynamic answer = null;
		var isExist = scope != null && scope.TryGetVariable("answer", out answer);
		if (!isExist)
		{
			warningText.text = "Your code is invalid!";
			warningText.color = Color.red;
			return;
		}
		steps = (IronPython.Runtime.List)answer();
		if (steps == null || !ValidateSteps(steps))
		{
			warningText.text = "The result of your code is invalid!";
			warningText.color = Color.red;
			return;
		}

		StartCoroutine(RunSteps());

		if (!CheckProblemSolved())
		{
			warningText.text = "Keep going! Problem is not solved.";
			warningText.color = Color.yellow;
		}
		else
		{
			warningText.text = "Well-done! Problem is solved";
			warningText.color = Color.green;
			nextButton.interactable = true;
		}
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

	IEnumerator RunSteps()
	{
		var stepCount = 1;
		foreach (PythonDictionary step in steps.Cast<PythonDictionary>())
		{
			var disk = (int)step["disk"];
			var from = (string)step["from"];
			var to = (string)step["to"];
			if (!MoveDisk(disk, from, to))
			{
				warningText.text = $"Fail! Stuck at step {stepCount}";
				warningText.color = Color.red;
				break;
			}
			stepCount++;
			yield return new WaitForSeconds(1.0f);
		}
	}


	private bool ValidateSteps(IronPython.Runtime.List steps)
	{
		return steps.All(step => step is IronPython.Runtime.PythonDictionary stepDict
			&& stepDict.has_key("disk") && stepDict.has_key("from") && stepDict.has_key("to"));
	}

	private bool CheckProblemSolved()
	{
		return disksOnRobC.All(disk => disk.name != "0");
	}

	private void ResetRobs()
	{
		UpdateDisk("A", 0, 1, new Color32(18, 77, 226, 255), new Vector2(45, 50));
		UpdateDisk("A", 1, 2, new Color32(62, 204, 38, 255), new Vector2(90, 50));
		UpdateDisk("A", 2, 3, new Color32(222, 204, 37, 255), new Vector2(135, 50));
		UpdateDisk("A", 3, 4, new Color32(215, 120, 21, 255), new Vector2(180, 50));
		UpdateDisk("A", 4, 5, new Color32(195, 38, 38, 255), new Vector2(225, 50));

		UpdateDisk("B", 0);
		UpdateDisk("B", 1);
		UpdateDisk("B", 2);
		UpdateDisk("B", 3);
		UpdateDisk("B", 4);

		UpdateDisk("C", 0);
		UpdateDisk("C", 1);
		UpdateDisk("C", 2);
		UpdateDisk("C", 3);
		UpdateDisk("C", 4);
	}

	private bool MoveDisk(int diskNum, string fromRob, string toRob)
	{
		var (fromRobIndex, objectToMove) = TopIndex(fromRob);
		if (fromRobIndex == -1 || objectToMove.name != diskNum.ToString())
			return false;
		var (toRobIndex, baseObject) = TopIndex(toRob, 5);
		if (toRobIndex <= 0 || (baseObject != null && int.Parse(baseObject.name) < diskNum))
			return false;
		UpdateDisk(toRob, toRobIndex - 1, diskNum,
			objectToMove.GetComponent<Image>().color,
			objectToMove.GetComponent<RectTransform>().sizeDelta);
		UpdateDisk(fromRob, fromRobIndex);
		return true;
	}

	private (int, GameObject) TopIndex(string rob, int defaultValue = -1)
	{
		return rob switch
		{
			"A" => TopIndex(disksOnRobA, defaultValue),
			"B" => TopIndex(disksOnRobB, defaultValue),
			"C" => TopIndex(disksOnRobC, defaultValue),
			_ => (defaultValue, null),
		};
	}

	private (int, GameObject) TopIndex(List<GameObject> list, int defaultValue = -1)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].name != "0")
				return (i, list[i]);
		}
		return (defaultValue, null);
	}

	private void UpdateDisk(string rob, int index, int diskNum = 0, Color32? color = null, Vector2? size = null)
	{
		GameObject disk = null;
		switch (rob)
		{
			case "A":
				disk = disksOnRobA[index];
				break;
			case "B":
				disk = disksOnRobB[index];
				break;
			case "C":
				disk = disksOnRobC[index];
				break;
		}
		disk.name = diskNum.ToString();
		var diskImg = disk.GetComponent<Image>();
		diskImg.color = color ?? new Color32(255, 255, 255, 0);
		var diskTransform = disk.GetComponent<RectTransform>();
		diskTransform.sizeDelta = size ?? new Vector2(45, 50);
	}

	private GameObject NewDisk(int diskNum = 0, Color32? color = null, Vector2? size = null)
	{
		var disk = new GameObject(diskNum.ToString());
		disk.AddComponent<Image>();
		var diskImg = disk.GetComponent<Image>();
		diskImg.color = color ?? new Color32(255, 255, 255, 0);
		var diskTransform = disk.GetComponent<RectTransform>();
		diskTransform.sizeDelta = size ?? new Vector2(45, 50);
		return disk;
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