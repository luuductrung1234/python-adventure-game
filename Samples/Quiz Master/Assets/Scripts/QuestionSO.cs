using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Question", menuName = "Quiz Question", order = 0)]
public class QuestionSO : ScriptableObject
{
	[TextArea(2, 6)]
	[SerializeField] string question = "Enter new question text here";
	[SerializeField] string[] answers = new string[4];
	[SerializeField] int correctAnswerIndex;

	public string GetQuestion()
	{
		return question;
	}

	public string[] GetAnswers()
	{
		return answers;
	}

	public int GetCorrectAnswerIndex()
	{
		return correctAnswerIndex;
	}
}