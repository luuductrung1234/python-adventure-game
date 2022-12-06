using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienCollision : MonoBehaviour
{
    
    public float Speed;
    public int life = 5;
    public UILabel LifeText; 
    float _x;
    float _y;
    public float jumppower ;
    bool pushFlag = false; 
    bool jumpFlag = false; 
    Rigidbody2D rbody;
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        
    }

   
    void Update()
    {
        _x = Input.GetAxisRaw("Horizontal");

        


        if (Input.GetKey("space"))
        {
            if (pushFlag == false)
            {
                jumpFlag = true; 
                pushFlag = true;
                _y = 1;
            }
        }
        else
        {
            pushFlag = false;
            _y = 0;
        }
        rbody.velocity = new Vector2(_x * Speed, _y*Speed);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.tag.Equals("Alien"))
        {
            life--;
            LifeText.text = life.ToString();
            //lifeText.text = ""+ life;
        }
    }
    void FixedUpdate()
    {

        transform.localPosition += new Vector3(_x * Speed * Time.deltaTime, _y * Speed * Time.deltaTime, 0);
        if (jumpFlag)
        {
            jumpFlag = false;
            rbody.AddForce(new Vector2(0, jumppower), ForceMode2D.Impulse);
        }
    }
}
