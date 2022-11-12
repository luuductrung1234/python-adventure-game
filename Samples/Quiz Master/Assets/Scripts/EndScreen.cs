using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI finalScoreText;
	[SerializeField] ScoreKeeper scoreKeeper;

	void Start()
	{
		finalScoreText.text = $"Congratulation!\nYou scored {scoreKeeper.CalculateScore()}%";
	}
}
