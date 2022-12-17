using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
	[SerializeField] TextMeshProUGUI finalScoreText;

	void Start()
	{
		finalScoreText.text = string.Empty;
	}

	public void SetScoreText(string text)
	{
		finalScoreText.text = text;
	}
}
