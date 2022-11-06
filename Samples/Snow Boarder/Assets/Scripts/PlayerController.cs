using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] float torqueAmount = 1f;
	[SerializeField] float boostSpeed = 30f;
	[SerializeField] float baseSpeed = 20f;

	private new Rigidbody2D rigidbody;
	private SurfaceEffector2D surfaceEffector2D;

	// Start is called before the first frame update
	internal void Start()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		surfaceEffector2D = FindObjectOfType<SurfaceEffector2D>();
	}

	private void FixedUpdate()
	{
		RotatePlayer();
		RespondToBoost();
	}

	private void RespondToBoost()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			surfaceEffector2D.speed = boostSpeed;
		}
		else
		{
			surfaceEffector2D.speed = baseSpeed;
		}
	}

	private void RotatePlayer()
	{
		var torque = (torqueAmount * Mathf.Deg2Rad) * rigidbody.inertia;

		if (Input.GetKey(KeyCode.LeftArrow))
		{
			rigidbody.AddTorque(torque, ForceMode2D.Impulse);
		}
		else if (Input.GetKey(KeyCode.RightArrow))
		{
			rigidbody.AddTorque((-1) * torque, ForceMode2D.Impulse);
		}
	}
}
