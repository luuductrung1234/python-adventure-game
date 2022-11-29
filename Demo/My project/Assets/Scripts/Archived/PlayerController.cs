using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed, jumpFroces;
    private float moveX;


    private const string WALK_ANIMATION = "Walk";
    private const string JUMP_ANIMATION = "JumpUp";
    private const string FALL_ANIMATION = "Fall";

    private Rigidbody2D myBody;
    private SpriteRenderer sr;
    private Animator anim;

    private GameController gameController;

    private void Awake() {
        myBody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    private void Update(){
        Walk();
    }

    private void LateUpdate(){
        PLayerJump();
    }


    private void Walk(){
        moveX = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(moveX, 0f, 0f)*Time.deltaTime*speed;
        Animate();
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

    bool isOnGround = true;
    void PLayerJump(){
        if (Input.GetButtonDown("Jump") && isOnGround){
            isOnGround = false;
            myBody.AddForce(new Vector2(0f, jumpFroces), ForceMode2D.Impulse);
            anim.SetBool("Jump", true);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Trap")){
            isOnGround = true;
            anim.SetBool("Jump", false);
        }
    }

}
