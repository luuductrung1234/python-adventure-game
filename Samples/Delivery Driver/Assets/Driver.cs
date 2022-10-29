using UnityEngine;

public class Driver : MonoBehaviour
{
	[SerializeField] private float steerSpeed = 300f;
	[SerializeField] private float moveSpeed = 20f;

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
}
