using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    [SerializeField]
    private string sceneName;

    private PlayerMovement pos;
    private Health hp;

    private void Awake() {
        pos = GameObject.FindObjectOfType<PlayerMovement>();
        hp = GameObject.FindObjectOfType<Health>();
    }

    private void OnMouseDown() {
        pos.UpdatePosition();
        hp.UpdateHealth();

        SceneManager.LoadScene(sceneName);
    }
}
