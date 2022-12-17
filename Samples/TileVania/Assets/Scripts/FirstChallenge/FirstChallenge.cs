using System;
using System.Collections.Generic;
using System.Linq;
using InGameCodeEditor;
using IronPython.Custom;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirstChallenge : MonoBehaviour
{
	private static string ERROR_MESSAGE = "Still have queens attack each other";

	[Header("Questions")]
	[SerializeField] TextMeshProUGUI questionText;
	[SerializeField] TextMeshProUGUI warningText;
	[SerializeField] GameObject descriptionView;
	[SerializeField] GameObject resultView;

	[Header("Answer By Code")]
	[SerializeField]
	private CodeEditor codeEditor;

	[Header("Chess Board")]
	[SerializeField]
	private List<GameObject> rows;
	[SerializeField]
	private Sprite whiteBlank;
	[SerializeField]
	private Sprite whiteChess;
	[SerializeField]
	private Sprite whiteChessError;
	[SerializeField]
	private Sprite greenBlank;
	[SerializeField]
	private Sprite greenChess;
	[SerializeField]
	private Sprite greenChessError;

	public Action onChallengeSolved;

	private void Start()
	{
		codeEditor.Text = seedAnswer;
		OnShowDescription();
		ResetBoard();
	}

	public void OnReset()
	{
		codeEditor.Text = seedAnswer;
		ResetBoard();
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
			return;
		}
		IronPython.Runtime.List rows = (IronPython.Runtime.List)answer();
		if (rows == null || rows.Count != 8 || rows.Any(row => ((string)row).Length != 8))
		{
			warningText.text = "The result of your code is invalid!";
			return;
		}
		var blockData = rows.Select(row => ((string)row).ToCharArray()).ToList();
		var problemNotSolved = false;
		for (int row = 0; row < 8; row++)
		{
			for (int column = 0; column < 8; column++)
			{
				var isError = !Validate(row, column, blockData);
				if (isError) problemNotSolved = true;
				updateBoardBlock(row, column, blockData[row][column] == 'Q', isError);
			}
		}
		if (problemNotSolved)
		{
			warningText.text = "Fail! Problem is not solved";
		}
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

	private bool Validate(int row, int column, List<char[]> blockData)
	{
		if (blockData[row][column] != 'Q')
			return true;
		for (int c = 0; c < 8; c++)
		{
			if (c == column)
				continue;
			if (blockData[row][c] == 'Q')
				return false;
		}
		for (int r = 0; r < 8; r++)
		{
			if (r == row)
				continue;
			if (blockData[r][column] == 'Q')
				return false;
		}
		for (int shift = 1; shift < 8; shift++)
		{
			int rightShift = column + shift;
			int leftShift = column - shift;
			int topShift = row - shift;
			int bottomShift = row + shift;
			if (rightShift >= 8 && leftShift < 0 && topShift < 0 && bottomShift >= 8)
				break;
			if (topShift >= 0 && rightShift < 8 && blockData[topShift][rightShift] == 'Q')
				return false;
			if (topShift >= 0 && leftShift >= 0 && blockData[topShift][leftShift] == 'Q')
				return false;
			if (bottomShift < 8 && rightShift < 8 && blockData[bottomShift][rightShift] == 'Q')
				return false;
			if (bottomShift < 8 && leftShift >= 0 && blockData[bottomShift][leftShift] == 'Q')
				return false;
		}
		return true;
	}

	private void ResetBoard()
	{
		for (int row = 0; row < 8; row++)
		{
			for (int column = 0; column < 8; column++)
			{
				updateBoardBlock(row, column);
			}
		}
	}

	private void updateBoardBlock(int row, int column, bool hasChessPiece = false, bool isError = false)
	{
		GameObject selectedRow = rows[row];
		Transform selectedColumn = selectedRow.transform.GetChild(column);
		Image img = selectedColumn.GetComponent<Image>();
		if ((row + column) % 2 == 0)
		{
			if (hasChessPiece && isError)
			{
				img.sprite = whiteChessError;
			}
			else if (hasChessPiece)
			{
				img.sprite = whiteChess;
			}
			else
			{
				img.sprite = whiteBlank;
			}
		}
		else
		{
			if (hasChessPiece && isError)
			{
				img.sprite = greenChessError;
			}
			else if (hasChessPiece)
			{
				img.sprite = greenChess;
			}
			else
			{
				img.sprite = greenBlank;
			}
		}
	}

	private const string seedAnswer = @"# import sys
# import System
# you can import build-in modules (of Python or .NET Clr)

# answer start

def answer():
	# write your own code to solve the challenge here...
	placement = [
		'........',
		'........',
		'........',
		'........',
		'........',
		'........',
		'........',
		'........'
	]
	return placement

# answer end
";

}