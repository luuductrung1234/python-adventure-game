using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LONLG_Camera : MonoBehaviour
{
    private Transform playerPos, doorX;
    private Vector3 tempPos; 

    public float leftX, rightX, minY;

    void Awake()
    {   
        playerPos = GameObject.Find("Player").transform;
    }
    void Update()
    {   
            tempPos = transform.position;
            tempPos.x = playerPos.position.x;
            tempPos.y = playerPos.position.y;

            if (tempPos.x < leftX){
                tempPos.x = (float) leftX;
            }
            if (tempPos.x > rightX){
                tempPos.x = (float) rightX;
            }
            if (tempPos.y < minY){
                tempPos.y = (float) minY;
            } 

            transform.position = Vector3.Lerp(transform.position, tempPos, 0.2f);
    }
}

