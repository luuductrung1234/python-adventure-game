using UnityEngine;

public static class Helpers
{
	public static float FrameIndependence(float value)
	{
		return value * Time.deltaTime;
	}
}