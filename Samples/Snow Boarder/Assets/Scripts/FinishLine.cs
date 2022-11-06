using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == Constants.PlayerTag)
		{
			SceneManager.LoadScene(Constants.Level1Scene);
		}
	}
}
