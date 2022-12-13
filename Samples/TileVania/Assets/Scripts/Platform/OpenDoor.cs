using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{

    public GameObject target;
    enum sectionNumber{
        section1,
        section2,
        section3,
    }

    [SerializeField]
    sectionNumber section = sectionNumber.section1;

    private void Update() {
        if (section == sectionNumber.section1 && SceneData.section1 == true){
            target.SetActive(true);
        }else if (section == sectionNumber.section2 && SceneData.section2 == true){
            target.SetActive(false);
        }else if (section == sectionNumber.section3 && SceneData.section3 == true){
            target.SetActive(false);
        }
    }
}
