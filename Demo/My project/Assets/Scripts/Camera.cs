using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private Transform playerPos, doorX;
    private Vector3 tempPos; 

    [SerializeField]
    public float? leftX = -2f, rightX = 2f, minY = 1f;


    void Awake()
    {
        playerPos = GameObject.FindWithTag("Player").transform;
    }
    void LateUpdate()
    {   
        if (GameObject.Find("Door") == null){
            rightX = null;
        }
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
