/***************************************************************
*Title: n/a
*Author: Pham Hoang Long
*Date: n/a
*Availability: n/a
*Code version: V1
****************************************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour
{
	public static int maxHealth = 5;
	public static int currentHealth = 0;
	public static ToScene currentScene = ToScene.MainScene;
	public static Vector3 playerMainScenePos;
	public static Vector3 playerLeonPos;
	public static Vector3 playerLilyPos;

	public static bool section1 = false;
	public static bool section2 = false;
	public static bool section3 = false;
	public static ChallengeMode challengeMode = ChallengeMode.Quiz;
}
