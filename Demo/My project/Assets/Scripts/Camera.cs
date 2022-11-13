using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private Transform playerPos, doorX;
    private Vector3 tempPos; 

    public float leftX = -2f, rightX = 2f, minY = 1f;

    void Awake()
    {   
        playerPos = GameObject.Find("Player").transform;
    }
    void LateUpdate()
    {   
        tempPos = transform.position;
        tempPos.x = playerPos.position.x;

        if (tempPos.x < leftX){
            tempPos.x = (float) leftX;
        }
        if (tempPos.x > rightX){
            tempPos.x = (float) rightX;
        }
        if (tempPos.y < minY){
            tempPos.y = (float) minY;
        } 

        transform.position = tempPos;
    }
}
