using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public bool isActive = true;
    enum trajectoryList{
        Horizontal,
        Vertical,
    }
    [SerializeField]
    trajectoryList trajectory = trajectoryList.Horizontal;
    [SerializeField]
    private float direction = 1;

    [SerializeField]
    private float speed, maxX, minX, maxY, minY;

    GameObject player;

    private void Awake() {
        player = GameObject.Find("Player");
    }

    private void Update() {
        if (isActive){
            startMoving();
        }
    }

    private void startMoving(){
        if (trajectory==trajectoryList.Horizontal){
            if (transform.localPosition.x >= maxX){
                direction = -1;
            }else if (transform.localPosition.x <= minX){
                direction = 1;
            }
            Vector3 tempos = transform.localPosition;
            tempos += new Vector3(direction, 0f, 0f);
            transform.localPosition = Vector3.Lerp(transform.localPosition, tempos, Time.deltaTime*speed);
        }
        else if (trajectory==trajectoryList.Vertical){
            if (transform.localPosition.y >= maxY){
                direction = -1;
            }else if (transform.localPosition.y <= minY){
                direction = 1;
            }
            Vector3 tempos = transform.localPosition;
            tempos += new Vector3(0f, direction, 0f);
            transform.localPosition = Vector3.Lerp(transform.localPosition, tempos, Time.deltaTime*speed);
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")){
            player.transform.parent = this.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player")){
            player.transform.parent = null;
        }
    }
}
