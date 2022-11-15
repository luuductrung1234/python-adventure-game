using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed, animSpeed;
    private float moveX;

    private string WALK_ANIMATION = "Walk";

    private Rigidbody2D myBody;
    private SpriteRenderer sr;
    private Animator anim;

    private GameController gameController;

    private void Awake() {
	    gameController = GameObject.FindObjectOfType<GameController>();
        myBody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void LateUpdate() {
        Walk();
    }


    private void Walk(){
        if(gameController.walkEnable){
            moveX = Input.GetAxisRaw("Horizontal");
            transform.position += new Vector3(moveX, 0f, 0f)*Time.deltaTime*speed;
            Animate();
        }
    }

    private void Animate(){
        if (moveX == 0){
            anim.SetBool(WALK_ANIMATION, false);
        }else{
            anim.SetBool(WALK_ANIMATION, true);
            if (moveX < 0){
                sr.flipX = true;
            }else{
                sr.flipX = false;
            }
        }

    }

}
