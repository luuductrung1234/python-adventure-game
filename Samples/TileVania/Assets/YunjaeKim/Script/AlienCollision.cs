using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class AlienCollision : MonoBehaviour
{
    
    public float Speed;
    public int life = 5;
    public UILabel LifeText;
    public UI2DSpriteAnimation playerWalk;
    public GameObject Anim_idle;
    public GameObject Anim_run;
    float _x;
    float _y;
    float _up;
    public float jumppower ;
    bool isGround = false;
    bool pushFlag = false; 
    bool jumpFlag = false; 
    Rigidbody2D rbody;
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        rbody.constraints = RigidbodyConstraints2D.FreezeRotation;
        transform.localPosition = Y_StaticClass.PLAYER_POSITION;
        
    }

    void Y_debug()
    {
        if (Input.GetKeyDown("1"))
        {
            SceneManager.LoadScene("MainScene");
        }
    }
    
    void Update()
    {
        Y_debug();
        _x = Input.GetAxisRaw("Horizontal");
        _up = Input.GetAxis("Vertical");
        Debug.Log(isGround);



        if (_x !=0) {
            Anim_idle.SetActive(false);
            Anim_run.SetActive(true);
            if (_x > 0)
            {
                transform.eulerAngles = Vector3.zero;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
        else
        {
            
            Anim_idle.SetActive(true);
            Anim_run.SetActive(false);
        }

        if (Input.GetKey("space"))
        {
            if (pushFlag == false&&isGround)
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
        if (IsStair)
        {
            print(_up);
            if (_up > 0)
            {
              transform.localPosition += new Vector3(0, _up * Speed * 400 * Time.deltaTime, 0);
            }else if (_up < 0)
            {
              transform.localPosition += new Vector3(0, _up * Speed * 1000 * Time.deltaTime, 0);
            }
           
            //rbody.velocity = new Vector2(_x * Speed, _up * Speed);
        }
        if (jumpFlag)
        {

            rbody.velocity = new Vector2(_x /4* Speed, _y * Speed);
        }
        else
        {
            rbody.velocity = new Vector2(_x * Speed, _y * Speed);
        }
       
        Y_StaticClass.PLAYER_POSITION = transform.localPosition;
    }

    bool IsStair=false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.tag.Equals("Alien"))
        {
            Y_StaticClass.PLAYER_LIFE--;
            if (Y_StaticClass.PLAYER_LIFE < 0)
            {
                // TODO: scence change
            }
            //lifeText.text = ""+ life;
        }
        if (collision.tag.Equals("Stair"))
        {
            IsStair = true;
        }
        if (collision.tag.Equals("Ground"))
        {
              isGround=true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Stair"))
        {
            IsStair = false;
        }
        if (collision.tag.Equals("Ground"))
        {
            isGround = false;
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

}
