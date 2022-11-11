using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnKeyPress_MoveGravity : MonoBehaviour
{
    public float speed=3;
    public float jumpPower=8;
    float vx=0;
    bool leftFlag=false;
    bool pushFlag=false;
    bool jumpFlag=false;
    bool groundFlag=false;
    Rigidbody2D rbody;

    void Start()
    {
        rbody=GetComponent<Rigidbody2D>();
        rbody.constraints=RigidbodyConstraints2D.FreezeRotation;
    }


    void Update()
    {
        vx=0;
        if(Input.GetKey("right")){
            vx=speed;
            leftFlag=false;
        }
        if(Input.GetKey("left")){
            vx=-speed;
            leftFlag=true;
        }
        if(Input.GetKey("space") && groundFlag){
            if(pushFlag==false){
                pushFlag=true;
                jumpFlag=true;
            }
        }else{
            pushFlag=false;
        }
    }
    void FixedUpdate(){
        rbody.velocity=new Vector2(vx,rbody.velocity.y);
        this.GetComponent<SpriteRenderer>().flipX=leftFlag;
        if(jumpFlag){
            jumpFlag=false;
            rbody.AddForce(new Vector2(0,jumpPower),ForceMode2D.Impulse);
        }
    }
    void OnTriggerExit2D(Collider2D other) {
    groundFlag=false;
    }
    void OnTriggerStay2D(Collider2D other) {
        groundFlag=true;
    }
    
}
