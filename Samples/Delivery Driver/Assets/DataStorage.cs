using System.Collections.Generic;

public static class DataStorage
{

	private static readonly System.Random random = new();

	private static int nextPackage = 0;
	private static int nextCustomer = 0;

	public static Customer GetNextCustomer()
	{
		var customer = Customers[nextCustomer];
		nextCustomer++;
		return customer;
	}

	public static Package GetNextPackage()
	{
		var package = Packages[nextPackage];
		nextPackage++;
		return package;
	}

	public static List<Customer> Customers { get; private set; } = new List<Customer>()
	{
		new Customer() {
			Address = "123 ABC"
		},
		new Customer() {
			Address = "256 BCD"
		},
		new Customer() {
			Address = "232 AEW"
		},
		new Customer() {
			Address = "018 YEF"
		},
		new Customer() {
			Address = "234 PDF"
		},
		new Customer() {
			Address = "583 YDG"
		},
	};

	public static List<Package> Packages { get; private set; } = new List<Package>() {
		new Package() {
			Address = "123 ABC",
			Point = 20,
		},
		new Package() {
			Address = "256 BCD",
			Point = 95
		},
		new Package() {
			Address = "232 AEW",
			Point = 50
		},
		new Package() {
			Address = "018 YEF",
			Point = 70
		},
		new Package() {
			Address = "234 PDF",
			Point = 70
		},
		new Package() {
			Address = "583 YDG",
			Point = 20
		},
	};
}

public class Customer
{
	public string Address { get; set; }
}

public class Package
{
	public string Address { get; set; }
	public int Point { get; set; }
}