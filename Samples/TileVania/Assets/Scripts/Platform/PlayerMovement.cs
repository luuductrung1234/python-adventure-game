using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed, jumpFroces;
    private float moveX;


    private const string WALK_ANIMATION = "Walk";
    private const string JUMP_ANIMATION = "Jump";
    private const string FALL_ANIMATION = "Fall";

    private Rigidbody2D myBody;
    private SpriteRenderer sr;
    private Animator anim;

    private void Awake()
    {
        myBody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        Walk();
    }

    private void LateUpdate()
    {
        PLayerJump();
    }


    private void Walk()
    {
        moveX = Input.GetAxisRaw("Horizontal");
        Vector3 tempos = transform.position;
        tempos += new Vector3(moveX, 0f, 0f);
        transform.position = Vector3.Lerp(transform.position, tempos, Time.deltaTime * speed);
        Animate();
    }

    private void Animate()
    {
        if (moveX == 0)
        {
            anim.SetBool(WALK_ANIMATION, false);
        }
        else
        {
            anim.SetBool(WALK_ANIMATION, true);
            if (moveX < 0)
            {
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }
        }
        if (myBody.velocity.y != 0)
        {
            isOnGround = false;
            anim.SetBool(JUMP_ANIMATION, true);
        }
        else
        {
            isOnGround = true;
            anim.SetBool(JUMP_ANIMATION, false);
        }
    }

    bool isOnGround = true;
    void PLayerJump()
    {
        if (Input.GetButtonDown(JUMP_ANIMATION) && isOnGround)
        {
            isOnGround = false;
            myBody.AddForce(new Vector2(0f, jumpFroces), ForceMode2D.Impulse);
            anim.SetBool(JUMP_ANIMATION, true);
        }
        anim.SetFloat("velocityY", myBody.velocity.y);
    }
}