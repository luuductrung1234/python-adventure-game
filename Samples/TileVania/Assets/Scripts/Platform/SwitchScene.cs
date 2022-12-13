using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    enum toScene{
        MainScene,
        LeonStage,
        YunjaeKimMap,
    }

    [SerializeField]
    toScene sceneName = toScene.MainScene;



    [SerializeField]
    private bool enableSection = false;
    enum enableMainSceneSection{
        section1,
        section2,
        section3,
    }

    [SerializeField]
    enableMainSceneSection section = enableMainSceneSection.section1;

    private PlayerMovement pos;
    private Health hp;

    private void Awake() {
        pos = GameObject.FindObjectOfType<PlayerMovement>();
        hp = GameObject.FindObjectOfType<Health>();
    }

    bool isCollide = false;
    private void LateUpdate() {
        switchScene();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player"){
            isCollide = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player"){
            isCollide = false;
        }
    }
    private void switchScene() {
        if (isCollide){
            if (Input.GetKeyDown(KeyCode.E)){
                pos.UpdatePosition();
                hp.UpdateHealth();

                if(enableSection){
                    if (section == enableMainSceneSection.section1){
                        SceneData.section1 = true;
                    }else if (section == enableMainSceneSection.section2){
                        SceneData.section2 = true;
                    }else if (section == enableMainSceneSection.section3){
                        SceneData.section3 = true;
                    }
                }
                SceneManager.LoadScene(sceneName.ToString());
            }
        }
    }
}
