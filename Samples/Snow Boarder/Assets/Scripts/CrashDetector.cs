using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashDetector : MonoBehaviour
{
	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == Constants.GroundTag)
		{
			SceneManager.LoadScene(Constants.Level1Scene);
		}
	}
}
