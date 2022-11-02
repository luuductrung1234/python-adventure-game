using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CustomerPackageSpawn : MonoBehaviour
{
	[SerializeField] List<GameObject> packageObjects;
	[SerializeField] List<GameObject> customerObjects;

	private static readonly System.Random random = new();
	private static readonly int spawnDistance = 8;

	private void Start()
	{
		var generatedCustomerObjects = customerObjects.Select(packageToSpawn =>
		{
			var house = RandomGetHouse();
			return GenerateCustomer(packageToSpawn, house);
		}).ToList();

		var generatedPackageObjects = packageObjects.Select(packageToSpawn =>
		{
			var road = RandomGetRoad();
			return GeneratePackage(packageToSpawn, road);
		}).ToList();

		generatedPackageObjects.ForEach(packageObject =>
		{
			var packageDataComponent = packageObject.GetComponent<PackageDataComponent>();
			var targetCustomerObject = generatedCustomerObjects.Where(customerObject =>
			{
				var customerDataComponent = customerObject.GetComponent<CustomerDataComponent>();
				return customerDataComponent.Address == packageDataComponent.Address;
			}).SingleOrDefault();
			packageDataComponent.Customer = targetCustomerObject;
		});
	}

	private GameObject GeneratePackage(GameObject packageToSpawn, Transform road)
	{
		var spawnParent = GetSpawnObject();
		var package = Instantiate(packageToSpawn, road.transform.position, packageToSpawn.transform.rotation, spawnParent);
		var packageDataComponent = package.GetComponent<PackageDataComponent>();
		var packageData = DataStorage.GetNextPackage();
		packageDataComponent.Address = packageData.Address;
		packageDataComponent.Point = packageData.Point;
		return package;
	}

	private GameObject GenerateCustomer(GameObject customerToSpawn, Transform house)
	{
		var spawnParent = GetSpawnObject();
		var customerPosition = house.transform.position + house.transform.up * spawnDistance;
		var customer = Instantiate(customerToSpawn, customerPosition, customerToSpawn.transform.rotation, spawnParent);
		var customerDataComponent = customer.GetComponent<CustomerDataComponent>();
		var customerData = DataStorage.GetNextCustomer();
		customerDataComponent.Address = customerData.Address;
		return customer;
	}

	private Transform RandomGetRoad()
	{
		while (true)
		{
			var roadIndex = random.Next(transform.childCount);
			var road = transform.GetChild(roadIndex);
			if (road.name.Contains("intersection", System.StringComparison.OrdinalIgnoreCase))
				continue;
			Debug.Log($"Selected object: {road.name} " +
				$"with x: {road.transform.position.x}, " +
				$"y: {road.transform.position.y}, " +
				$"z: {road.transform.position.z}.");
			return road;
		}
	}

	private Transform RandomGetHouse()
	{
		while (true)
		{
			var houseParent = GetHousesObject();
			var houseIndex = random.Next(houseParent.childCount);
			var house = houseParent.GetChild(houseIndex);
			Debug.Log($"Selected object: {house.name} " +
				$"with x: {house.transform.position.x}, " +
				$"y: {house.transform.position.y}, " +
				$"z: {house.transform.position.z}.");
			return house;
		}
	}

	private Transform GetHousesObject() => transform.parent.Find("Houses");
	private Transform GetSpawnObject() => transform.parent.Find("Spawn");
}