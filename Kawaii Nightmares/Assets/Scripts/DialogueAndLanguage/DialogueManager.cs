using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements; 
using TMPro;

//[RequireComponent(typeof(TextMeshProUGUI))]
public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    public Animator animator; 

    private Queue<string> sentences; 

    // Start is called before the first frame update
    void Start()
    {
        sentences = new Queue<string>();
    }

   public void StartDialogue(Dialogue dialogue)
   {
        animator.SetBool("IsOpen", true); 
        nameText.text = dialogue.name; 

        sentences.Clear();
        foreach(string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence); 
        }

        DisplayNextSentence();

   }
   public void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        string value= LocalisationSystem.GetLocalisedValue(sentence);
        StartCoroutine(TypeSentence(value)); 
    }
    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray()) 
        {
            dialogueText.text += letter;
            yield return null; 
        
        }
    }
    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
    }
}
