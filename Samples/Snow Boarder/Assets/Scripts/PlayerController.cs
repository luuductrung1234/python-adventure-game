using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] float torqueAmount = 1f;

	private new Rigidbody2D rigidbody;

	// Start is called before the first frame update
	internal void Start()
	{
		rigidbody = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
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
