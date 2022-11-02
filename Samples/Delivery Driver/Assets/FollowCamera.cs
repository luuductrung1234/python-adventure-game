using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
	[SerializeField] internal GameObject followObject;

	// Update is called once per frame
	private void LateUpdate()
	{
		transform.position = followObject.transform.position + new Vector3(0, 0, -50);
	}
}
