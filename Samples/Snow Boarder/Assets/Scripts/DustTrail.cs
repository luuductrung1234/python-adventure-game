using UnityEngine;

public class DustTrail : MonoBehaviour
{
	[SerializeField] ParticleSystem dustEffect;

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == Constants.GroundTag)
			dustEffect.Play();
	}

	private void OnCollisionExit2D(Collision2D other)
	{
		if (other.gameObject.tag == Constants.GroundTag)
			dustEffect.Stop();
	}
}
