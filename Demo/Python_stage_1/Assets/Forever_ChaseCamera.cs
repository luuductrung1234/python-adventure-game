using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Forever_ChaseCamera : MonoBehaviour
{
    // Vector3 base_pos;
    void Start()
    {
        
    }

    void LateUpdate()
    {
        Vector3 pos = this.transform.position;
        pos.z = -10;
        // pos.y = base_pos.y;
        Camera.main.gameObject.transform.position = pos;
    }
}
