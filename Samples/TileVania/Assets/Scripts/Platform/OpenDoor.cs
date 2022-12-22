using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{   

    public GameObject door1;
    public GameObject door2;
    public GameObject door3;
    private void Update() {
        if (SceneData.section1 == true){
            door1.SetActive(true);
        }
        if (SceneData.section2 == true){
            door2.SetActive(false);
        }
        if (SceneData.section3 == true){
            door3.SetActive(false);
        }
    }
}
