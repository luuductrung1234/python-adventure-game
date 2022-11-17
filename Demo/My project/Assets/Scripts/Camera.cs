using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    private Transform playerPos, doorX;
    private Vector3 tempPos; 
    private GameController gameController;

    public float leftX, rightX, minY = 1f;

    void Awake()
    {   
        playerPos = GameObject.Find("Player").transform;
        gameController = GameObject.FindObjectOfType<GameController>();
    }
    void Update()
    {   
        if (!gameController.isFreezeCam){
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

            transform.position = tempPos;
        }   
    }
}
