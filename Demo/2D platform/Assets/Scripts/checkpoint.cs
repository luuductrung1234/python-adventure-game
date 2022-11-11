using UnityEngine;

public class checkpoint : MonoBehaviour
{
    [SerializeField] private Transform PreviousRoom;
    [SerializeField] private Transform NextRoom;
    [SerializeField] private CameraControl cam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.transform.position.x < transform.position.x)
                cam.MoveToNewRoom(NextRoom);
            else
                cam.MoveToNewRoom(PreviousRoom);
        }
    }

    
}