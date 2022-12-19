using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using InGameCodeEditor;
using IronPython.Custom;
using IronPython.Runtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ThirdChallenge : MonoBehaviour
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
	[SerializeField] List<GameObject> rows;

	[Header("Assets")]
	[SerializeField] Sprite blank;
	[SerializeField] Sprite start;
	[SerializeField] Sprite end;
	[SerializeField] Sprite obstacle;
	[SerializeField] Sprite visitedBlank;
	[SerializeField] Sprite visitedStart;
	[SerializeField] Sprite visitedEnd;

	private IronPython.Runtime.List steps;

	public Action onChallengeSolved;

	private void Start()
	{
		codeEditor.Text = seedAnswer;
		warningText.text = string.Empty;
		nextButton.interactable = false;
		OnShowDescription();
		ResetMaze();
	}

	public void OnReset()
	{
		codeEditor.Text = seedAnswer;
		warningText.text = string.Empty;
		nextButton.interactable = false;
		ResetMaze();
	}

	public void OnAnswered()
	{
		ResetMaze();
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

	private bool CheckProblemSolved()
	{
		return new List<GameObject>() {
			GetCell(0, 0),
			GetCell(0, 1),
			GetCell(2, 1),
			GetCell(3, 1),
			GetCell(4, 1),
			GetCell(4, 2),
			GetCell(4, 3),
		}.All(cell => cell.tag != null && cell.tag == "Visited");
	}

	IEnumerator RunSteps()
	{
		var stepCount = 1;
		foreach (PythonDictionary step in steps.Cast<PythonDictionary>())
		{
			var column = (int)step["x"];
			var row = (int)step["y"];
			var cell = GetCell(row, column);
			if (cell == null || cell.name == "x")
			{
				warningText.text = $"Fail! Stuck at step {stepCount}";
				warningText.color = Color.red;
				break;
			}
			switch (cell.name)
			{
				case "S":
					cell.GetComponent<Image>().sprite = visitedStart;
					cell.tag = "Visited";
					break;
				case "E":
					cell.GetComponent<Image>().sprite = visitedEnd;
					cell.tag = "Visited";
					break;
				case ".":
					cell.GetComponent<Image>().sprite = visitedBlank;
					cell.tag = "Visited";
					break;
			}
			stepCount++;
			yield return new WaitForSeconds(1.0f);
		}

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

	private bool ValidateSteps(IronPython.Runtime.List steps)
	{
		return steps.All(step => step is IronPython.Runtime.PythonDictionary stepDict
			&& stepDict.has_key("x") && stepDict.has_key("y"));
	}

	private void ResetMaze()
	{
		rows.SelectMany(row => GetCells(row)).ToList().ForEach(cell =>
		{
			cell.name = ".";
			cell.tag = string.Empty;
			cell.GetComponent<Image>().sprite = blank;
		});

		var startCell = GetCell(0, 0);
		startCell.name = "S";
		startCell.GetComponent<Image>().sprite = start;

		var endCell = GetCell(4, 3);
		endCell.name = "E";
		endCell.GetComponent<Image>().sprite = end;

		new List<GameObject>() {
			GetCell(2, 0),
			GetCell(3, 0),
			GetCell(4, 0),
			GetCell(1, 2),
			GetCell(2, 2),
			GetCell(3, 2),
			GetCell(1, 3),
			GetCell(2, 3),
		}.ForEach(cell =>
		{
			cell.name = "x";
			cell.GetComponent<Image>().sprite = obstacle;
		});
	}

	private List<GameObject> GetCells(GameObject row)
	{
		return Enumerable.Cast<Transform>(row.transform)
			.Select(cell => cell.gameObject)
			.ToList();
	}

	private GameObject GetCell(int rowIndex, int columnIndex)
	{
		if (rowIndex < 0 || rowIndex >= rows.Count)
			return null;
		var row = rows[rowIndex];
		var cells = GetCells(row);
		if (columnIndex < 0 || columnIndex >= cells.Count)
			return null;
		return cells[columnIndex];
	}

	private const string seedAnswer = @"# import sys
# import System
# you can import build-in modules (of Python or .NET Clr)

# constants
R = C = 5
m = [
	['.','.','.','.','.'],
	['.','.','x','x','.'],
	['x','.','x','x','.'],
	['x','.','x','.','.'],
	['x','.','.','E','.']
]
dc = [1, -1, 0, 0]
dr = [0, 0, -1, 1]

# tracking variables
rq = []
cq = []
visited = [
  [False, False, False, False, False],
  [False, False, False, False, False],
  [False, False, False, False, False],
  [False, False, False, False, False],
  [False, False, False, False, False]
]
moved_count = 0
nodes_left_in_layer = 0
nodes_in_next_layer = 0
reached_end = False
path = []

#========================================
# start answer

def answer():
  global path
  explore_maze(0, 0)
  return path

def explore_maze(sr, sc):
  '''
  sr: y co-ordinate of the beginning location
  sc: x co-ordinate of the beginning location
  '''
  path.append({'x': sr, 'y': sc})
  path.append({'x': 1, 'y': 0})
  path.append({'x': 1, 'y': 1})
  
# end answer
#========================================

def explore_neighbors(r, c):
	global R, C, dc, dr, m
	global rq, cq, visited, nodes_in_next_layer, path
	for i in range(4):
		nr = r + dr[i]
		nc = c + dc[i]
		if nr < 0 or nr >= R:
			continue
		if nc < 0 or nc >= C:
			continue
		if visited[nr][nc]:
			continue
		if m[nr][nc] == 'x':
			continue
		rq.append(nr)
		cq.append(nc)
		visited[nr][nc] = True
		path.append({'x': nc, 'y': nr})
		nodes_in_next_layer += 1

";
}