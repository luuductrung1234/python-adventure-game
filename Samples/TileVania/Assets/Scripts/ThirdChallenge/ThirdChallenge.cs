using System;
using InGameCodeEditor;
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

	public Action onChallengeSolved;

	private void Start()
	{
		codeEditor.Text = seedAnswer;
		warningText.text = string.Empty;
		nextButton.interactable = false;
		OnShowDescription();
	}

	public void OnReset()
	{
		codeEditor.Text = seedAnswer;
		warningText.text = string.Empty;
		nextButton.interactable = false;
	}

	public void OnAnswered()
	{

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