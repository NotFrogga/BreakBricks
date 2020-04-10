using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    TextMeshProUGUI npcText;
    TextMeshProUGUI dialogueText;
    [SerializeField] Animator animator;
    Animator paddleAnimator;
    private Queue<string> sentences = new Queue<string>();

    private void Start()
    {
        paddleAnimator = FindObjectOfType<Paddle>().GetComponent<Animator>();
        npcText = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>()
                 .FirstOrDefault(g => g.CompareTag("NPCName"));
        dialogueText = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>()
                 .FirstOrDefault(g => g.CompareTag("DialogueText"));
    }

    public void StartDialogue(Dialogue dialogue)
    {
        paddleAnimator.SetBool("isTalking", true);
        animator.SetBool("isOpen", true);
        npcText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }
    
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray()){

            GetComponent<AudioSource>().Play();
            dialogueText.text += letter;
            yield return null;
        }

    }
    public void EndDialogue()
    {
        animator.SetBool("isOpen", false);
        paddleAnimator.SetBool("isTalking", false);
    }
}
