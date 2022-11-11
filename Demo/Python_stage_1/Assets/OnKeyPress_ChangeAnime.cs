using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OnKeyPress_ChangeAnime : MonoBehaviour
{
    
    public string upAnime = "";     
    public string downAnime = "";   
    public string rightAnime = "";  
    public string leftAnime = "";   

    string nowMode = "";

    void Start() 
    { 
        nowMode = "";

    }

    void Update()
    { 
        nowMode="";
        if (Input.GetKey("up"))
        { 
            nowMode = upAnime;
        }
        if (Input.GetKey("down"))
        { 
            nowMode = downAnime;
        }
        if (Input.GetKey("right"))
        { 
            nowMode = rightAnime;
        }
        if (Input.GetKey("left"))
        { 
            nowMode = leftAnime;
        }
    }
    void FixedUpdate() 
    { 
        
        if (nowMode != "")
        {
    
            Animator animator = this.GetComponent<Animator>();
            animator.Play(nowMode);
        }
    }
}
