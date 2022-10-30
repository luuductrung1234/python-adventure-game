using System.Collections.Generic;
using UnityEngine;

public class PackageSpawn : MonoBehaviour
{
	[SerializeField] List<GameObject> packageObjects;

	private static readonly System.Random random = new();

	private void Start()
	{
		packageObjects.ForEach(packageToSpawn =>
		{
			var road = RandomGetRoad();
			GeneratePackage(packageToSpawn, road);
		});
	}

	private GameObject GeneratePackage(GameObject packageToSpawn, Transform road)
	{
		var spawnParent = GetSpawnObject();
		var package = Instantiate(packageToSpawn, road.transform.position, packageToSpawn.transform.rotation, spawnParent);
		var packageData = package.GetComponent<PackageDataComponent>();
		packageData.Address = "123 NVQ";
		packageData.Point = 100;
		return package;
	}

	private Transform RandomGetRoad()
	{
		while (true)
		{
			var roadIndex = random.Next(transform.childCount + 1);
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

	private Transform GetSpawnObject() => transform.parent.Find("Spawn");
}