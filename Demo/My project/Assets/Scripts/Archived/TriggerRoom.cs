using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRoom : MonoBehaviour
{
    public Dialogue myDialogue;


    // run dialogue for room 1
    private bool triggered = false;
    private void OnTriggerEnter2D(Collider2D other) {
        if (!triggered){
            myDialogue.NextLine();
            triggered = true;
        }
    }
}
