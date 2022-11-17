using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRoom : MonoBehaviour
{
    public Dialogue myDialogue;

    private bool triggered = false;
    private void OnTriggerEnter2D(Collider2D other) {
        if (!triggered){
            myDialogue.NextLine();
            triggered = true;
        }
    }
}
