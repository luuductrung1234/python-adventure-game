using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
	[SerializeField] AudioSource victorySound;
	[SerializeField] ParticleSystem finishEffect;
	[SerializeField] float loadDelay = 2f;

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == Constants.PlayerTag)
		{
			finishEffect.Play();
			Invoke("ReloadScene", loadDelay);
			victorySound.Play();
		}
	}

	private void ReloadScene()
	{
		SceneManager.LoadScene(Constants.Level1Scene);
	}
}
