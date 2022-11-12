using TMPro;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI scoreText;
	private int correctAnswers = 0;
	private int questionsSeen = 0;

	public void IncreaseCorrectAnswer()
	{
		correctAnswers++;
		CalculateScore();
	}

	public void IncreaseQuestionSeen()
	{
		questionsSeen++;
	}

	private void CalculateScore()
	{
		var score = Mathf.RoundToInt(correctAnswers / (float)questionsSeen * 100);
		Debug.Log($"Score: {score}%");
		scoreText.text = $"Score: {score}%";
	}

	public void Reset()
	{
		correctAnswers = 0;
		questionsSeen = 0;
	}
}
