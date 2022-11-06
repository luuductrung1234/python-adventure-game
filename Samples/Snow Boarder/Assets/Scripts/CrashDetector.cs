using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashDetector : MonoBehaviour
{
	[SerializeField] AudioSource crashSound;
	[SerializeField] ParticleSystem crashEffect;
	[SerializeField] float loadDelay = 0.7f;

	private bool crashed = false;

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (!crashed && other.gameObject.tag == Constants.GroundTag)
		{
			crashEffect.Play();
			Invoke("ReloadScene", loadDelay);
			crashSound.Play();
			crashed = true;
		}
	}

	private void ReloadScene()
	{
		SceneManager.LoadScene(Constants.Level1Scene);
	}
}
