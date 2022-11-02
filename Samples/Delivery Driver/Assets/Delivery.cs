using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : MonoBehaviour
{
	private string Address;

	internal void OnCollisionEnter2D(Collision2D other)
	{
		Debug.Log("Collision Enter 2D occurred");
	}

	internal void OnTriggerEnter2D(Collider2D other)
	{
		switch (other.tag)
		{
			case Constants.CustomerTag:
				var customerData = other.GetComponent<CustomerDataComponent>();
				if (Address != customerData.Address)
				{
					Debug.Log($"Reach customer at {customerData.Address}, but not the correct destination, we need to go to {Address}.");
					return;
				}
				Debug.Log("Delivered to the customer");
				Address = string.Empty;
				Destroy(other.gameObject);
				break;
			case Constants.PackageTag:
				if (!string.IsNullOrWhiteSpace(Address))
					return;
				var packageData = other.GetComponent<PackageDataComponent>();
				Debug.Log($"Picked up a package. Package info is address: {packageData.Address}");
				Address = packageData.Address;
				var customerRenderer = packageData.Customer.GetComponent<SpriteRenderer>();
				customerRenderer.color = Color.yellow;
				Destroy(other.gameObject);
				break;
			default:
				throw new InvalidOperationException($"GameObject's tag: {other.tag} is not supported!");
		}
	}
}
