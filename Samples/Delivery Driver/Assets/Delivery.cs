using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : MonoBehaviour
{
	internal void OnCollisionEnter2D(Collision2D other)
	{
		Debug.Log("Collision Enter 2D occurred");
	}

	internal void OnTriggerEnter2D(Collider2D other)
	{
		switch (other.tag)
		{
			case Constants.CustomerTag:
				Debug.Log("Delivered to the customer");
				break;
			case Constants.PackageTag:
				var packageData = other.GetComponent<PackageDataComponent>();
				Debug.Log($"Picked up a package. Package info is address: {packageData.Address}");
				break;
			default:
				throw new InvalidOperationException($"GameObject's tag: {other.tag} is not supported!");
		}
	}
}
