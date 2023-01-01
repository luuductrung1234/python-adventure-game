/***************************************************************
*Title: n/a
*Author: Luu Duc Trung
*Date: n/a
*Availability: n/a
*Code version: n/a
****************************************************************/

using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI finalScoreText;
	private string text;

	void Start()
	{
		finalScoreText.text = string.Empty;
	}

	private void Update()
	{
		finalScoreText.text = this.text;
	}

	public void SetScoreText(string text)
	{
		this.text = text;
	}
}
