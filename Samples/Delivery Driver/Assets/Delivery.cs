using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delivery : MonoBehaviour
{
	[SerializeField] Color32 hasPackageColor = new Color32(1, 1, 1, 1);
	[SerializeField] Color32 noPackageColor = new Color32(1, 1, 1, 1);

	private string Address;
	private SpriteRenderer driverRenderer;

	internal void Start()
	{
		driverRenderer = GetComponent<SpriteRenderer>();
	}

	internal void OnCollisionEnter2D(Collision2D other)
	{
		Debug.Log("Collision Enter 2D occurred");
	}

	internal void OnTriggerEnter2D(Collider2D other)
	{
		switch (other.tag)
		{
			case Constants.CustomerTag:
				reachCustomer(other);
				break;
			case Constants.PackageTag:
				reachPackage(other);
				break;
			default:
				throw new InvalidOperationException($"GameObject's tag: {other.tag} is not supported!");
		}
	}

	private void reachCustomer(Collider2D customer)
	{
		var customerData = customer.GetComponent<CustomerDataComponent>();
		if (Address != customerData.Address)
		{
			Debug.Log($"Reach customer at {customerData.Address}, but not the correct destination, we need to go to {Address}.");
			return;
		}
		Debug.Log("Delivered to the customer");
		Address = string.Empty;
		driverRenderer.color = noPackageColor;
		Destroy(customer.gameObject);
	}

	private void reachPackage(Collider2D package)
	{
		if (!string.IsNullOrWhiteSpace(Address))
			return;
		var packageData = package.GetComponent<PackageDataComponent>();
		Debug.Log($"Picked up a package. Package info is address: {packageData.Address}");
		Address = packageData.Address;
		var customerRenderer = packageData.Customer.GetComponent<SpriteRenderer>();
		customerRenderer.color = Color.yellow;
		driverRenderer.color = hasPackageColor;
		Destroy(package.gameObject);
	}
}
