using UnityEngine;

public class Driver : MonoBehaviour
{
	[SerializeField] private float steerSpeed = 150f;
	[SerializeField] private float moveSpeed = 17f;
	[SerializeField] private float slowSpeed = 14f;
	[SerializeField] private float boostSpeed = 30f;

	// Start is called before the first frame update
	void Start()
	{
		Debug.Log($"{nameof(Driver)} is initialized!");
	}

	// Update is called once per frame
	void Update()
	{
		var steerAmount = -Helpers.FrameIndependence(Input.GetAxis(Constants.HorizontalAxis) * steerSpeed);
		var moveAmount = Helpers.FrameIndependence(Input.GetAxis(Constants.VerticalAxis) * moveSpeed);
		if (moveAmount < 0)
		{
			steerAmount = -steerAmount;
		}
		transform.Rotate(0, 0, steerAmount);
		transform.Translate(0, moveAmount, 0);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == Constants.TreeTag
			|| other.gameObject.tag == Constants.RockTag
			|| other.gameObject.tag == Constants.HorizontalAxis)
		{
			moveSpeed = slowSpeed;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == Constants.BoosterTag)
		{
			moveSpeed = boostSpeed;
		}
	}
}
