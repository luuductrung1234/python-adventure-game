using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public Text myText;
    public string[] lines;
    public float textSpeed;

    private int index;

    private void Start() {
        myText.text = string.Empty;
        StartDialogue();
    }

    public void StartDialogue(){
        index = 0;
        StartCoroutine(TypeLine());
    }

    public bool textLoading;
    IEnumerator TypeLine(){
        foreach (char c in lines[index].ToCharArray()){
            myText.text += c;
            yield return new WaitForSeconds(textSpeed);   
        }
    }

    public void NextLine(){
        if (index < lines.Length - 1){
            index++;
            textLoading = true;
            myText.text = string.Empty;
            StartCoroutine(TypeLine());
        }else{
            gameObject.SetActive(false);
        }
    }

    public void SkipText(){
        StopAllCoroutines();
        myText.text = lines[index];
        textLoading = false;
    }
}
