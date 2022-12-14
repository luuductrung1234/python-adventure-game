/***************************************************************
*Title: Learn Unity - Beginner's Game Development Tutorial
*Author: Fahir from Awesome Tuts
*Date: 15 Apirl, 2021
*Availability: https://youtu.be/gB1F9G0JXOo?t=13826 
****************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed, jumpFroces;
    private float moveX;
    private bool freeze = false;


    private const string WALK_ANIMATION = "Walk";
    private const string JUMP_ANIMATION = "Jump";
    private const string FALL_ANIMATION = "Fall";

    private Rigidbody2D myBody;
    private SpriteRenderer sr;
    private Animator anim;

    [SerializeField]
    private AudioSource jumpSound;

    string currentScene;

    private void Awake() {
        myBody = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Start() {
        string scene = SceneManager.GetActiveScene().name;
        currentScene = scene;
        if (scene == "MainScene" && SceneData.playerMainScenePos != new Vector3(0f, 0f, 0f)){
            transform.position = SceneData.playerMainScenePos;
        }
        else if (scene == "LeonStage" && SceneData.playerLeonPos != new Vector3(0f, 0f, 0f)){
            transform.position = SceneData.playerLeonPos;
        }
        else if (scene == "YunjaeKimMap" && SceneData.playerLilyPos != new Vector3(0f, 0f, 0f)){
            transform.position = SceneData.playerLilyPos;
        }
    }

    private void Update(){
        if (!freeze){
            Walk();
        }
    }

    private void LateUpdate(){
        PLayerJump();
    }

    private void Walk(){
        moveX = Input.GetAxisRaw("Horizontal");
        Vector3 tempos = transform.position;
        tempos += new Vector3(moveX, 0f, 0f);
        transform.position = Vector3.Lerp(transform.position, tempos, Time.deltaTime*speed);
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
        if (myBody.velocity.y != 0){
            isOnGround = false;
            anim.SetBool(JUMP_ANIMATION, true);
        }else{
            isOnGround = true;
            anim.SetBool(JUMP_ANIMATION, false);
        } 
    }

    bool isOnGround = true;
    void PLayerJump(){
        if (Input.GetButtonDown(JUMP_ANIMATION) && isOnGround){
            jumpSound.Play();
            isOnGround = false;
            myBody.AddForce(new Vector2(0f, jumpFroces), ForceMode2D.Impulse);
            anim.SetBool(JUMP_ANIMATION, true);
        }
        anim.SetFloat("velocityY", myBody.velocity.y);
    }

    public void UpdatePosition(){
        switch (currentScene){
            case "MainScene":
                SceneData.playerMainScenePos = transform.position;
                break;
            case "LeonStage":
                SceneData.playerLeonPos = transform.position;
                break;
            case "YunjaeKimMap":
                SceneData.playerLilyPos = transform.position;
                break;
        }
    }
}
