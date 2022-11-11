using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forever_MoveV : MonoBehaviour
{

    public float speed = 1; 

    void FixedUpdate()
    { 
        this.transform.Translate(0 ,speed/50, 0); 
    }
}

