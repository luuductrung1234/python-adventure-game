using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
	internal void OnCollisionEnter2D(Collision2D other)
	{
		Debug.Log("Collision Enter 2D occurred");
	}

	internal void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("Trigger Enter 2D occurred");
	}
}
