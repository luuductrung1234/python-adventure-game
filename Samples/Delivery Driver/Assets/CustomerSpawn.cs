using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawn : MonoBehaviour
{
	[SerializeField] List<GameObject> customerObjects;

	private static readonly System.Random random = new();
	private static readonly int spawnDistance = 20;

	private void Start()
	{
		customerObjects.ForEach(packageToSpawn =>
		{
			var house = RandomGetHouse();
			GenerateCustomer(packageToSpawn, house);
		});
	}

	private GameObject GenerateCustomer(GameObject customerToSpawn, Transform house)
	{
		var spawnParent = GetSpawnObject();
		var customerPosition = house.transform.position + house.transform.forward * spawnDistance;
		var customer = Instantiate(customerToSpawn, customerPosition, customerToSpawn.transform.rotation, spawnParent);
		var customerData = customer.GetComponent<CustomerDataComponent>();
		customerData.Address = "123 NVQ";
		return customer;
	}

	private Transform RandomGetHouse()
	{
		while (true)
		{
			var houseIndex = random.Next(transform.childCount + 1);
			var house = transform.GetChild(houseIndex);
			if (house.name.Contains("intersection", System.StringComparison.OrdinalIgnoreCase))
				continue;
			Debug.Log($"Selected object: {house.name} " +
				$"with x: {house.transform.position.x}, " +
				$"y: {house.transform.position.y}, " +
				$"z: {house.transform.position.z}.");
			return house;
		}
	}

	private Transform GetSpawnObject() => transform.parent.Find("Spawn");
}