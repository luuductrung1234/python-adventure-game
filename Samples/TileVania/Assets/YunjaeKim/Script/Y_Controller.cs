using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_Controller : MonoBehaviour
{
    public UI2DSprite[] Life;
    public Sprite enableLife;
    public Sprite disableLife;


    int currentLife = 0;

    void UpdateLife()
    {
        if (currentLife != Y_StaticClass.PLAYER_LIFE) 
        {
            currentLife = Y_StaticClass.PLAYER_LIFE;
            foreach(UI2DSprite i in Life)
            {
                i.sprite2D = disableLife;
            }
            for(int i = 0; i < Y_StaticClass.PLAYER_LIFE; i++)
            {
                Life[i].sprite2D = enableLife;
            }
        }
          
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLife();
        if (Input.GetKeyDown("2"))
        {
            Y_StaticClass.PLAYER_LIFE = 1;
        }
        if (Input.GetKeyDown("4"))
        {
            Y_StaticClass.PLAYER_LIFE = 4;
        }
    }
}
