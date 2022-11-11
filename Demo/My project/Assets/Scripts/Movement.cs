using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    [SerializeField]
    private float speed = 10f;
    
    private GameController gameController;

    private void Awake() {
        gameController = GameObject.FindObjectOfType<GameController>();
    }
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   
        if (!gameController.isGameOver){
            Walk();
        }
        if (gameController.isGameOver){
            GameObject.Find("Player").SetActive(false);
        }
    }

    private float moveX;
    void Walk(){
        moveX = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(moveX, 0f, 0f)*Time.deltaTime*speed;
    }
    
}
