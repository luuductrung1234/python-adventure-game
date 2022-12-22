using UnityEngine;
using UnityEngine.SceneManagement;

public enum ToScene
{
	MainScene,
	LeonStage,
	YunjaeKimMap,
	TinaStage,
	SampleQuizScene
}

public enum EnableMainSceneSection
{
	None,
	Section1,
	Section2,
	Section3,
}

public class SwitchScene : MonoBehaviour
{
	[SerializeField]
	ToScene sceneName = ToScene.MainScene;

	[SerializeField]
	private bool enableSection = false;

	[SerializeField]
	EnableMainSceneSection section = EnableMainSceneSection.None;

	private PlayerMovement _pos;
	private Health _hp;
	private bool isCollide = false;

	private void Awake()
	{
		_pos = GameObject.FindObjectOfType<PlayerMovement>();
		_hp = GameObject.FindObjectOfType<Health>();
	}

	private void LateUpdate()
	{
		DoSwitchScene();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			isCollide = true;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			isCollide = false;
		}
	}

	private void DoSwitchScene()
	{
		if (isCollide)
		{
			if (Input.GetKeyDown(KeyCode.E))
			{
				_pos.UpdatePosition();
				_hp.UpdateHealth();

				SceneData.currentScene = sceneName;

				if (enableSection && section != EnableMainSceneSection.None)
				{
					if (section == EnableMainSceneSection.Section1 && !SceneData.section1)
					{
						SceneData.section1 = true;
						SceneData.challengeMode = ChallengeMode.FirstChallenge;
						SceneManager.LoadScene(ToScene.SampleQuizScene.ToString());
					}
					else if (section == EnableMainSceneSection.Section2 && !SceneData.section2)
					{
						SceneData.section2 = true;
						SceneData.challengeMode = ChallengeMode.SecondChallenge;
						SceneManager.LoadScene(ToScene.SampleQuizScene.ToString());
					}
					else if (section == EnableMainSceneSection.Section3 && !SceneData.section3)
					{
						SceneData.section3 = true;
						SceneData.challengeMode = ChallengeMode.ThirdChallenge;
						SceneManager.LoadScene(ToScene.SampleQuizScene.ToString());
					}
				}
				else
				{
					SceneManager.LoadScene(SceneData.currentScene.ToString());
				}
			}
		}
	}
}
