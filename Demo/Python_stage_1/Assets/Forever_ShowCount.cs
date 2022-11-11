using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Forever_ShowCount : MonoBehaviour
{

    void Update()
    { 
        GetComponent<Text>().text = GameCounter.value.ToString();
    }
}
