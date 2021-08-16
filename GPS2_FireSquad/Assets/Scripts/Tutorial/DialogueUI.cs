using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private DialogueObject textDialogue;

    private TypewritingEffect typewritingEffect;
    public bool dialogueBoxisOpen = false;

    private void Start()
    {
        typewritingEffect = GetComponent<TypewritingEffect>();
        //CloseDialogueBox();
        //ShowDialogue(textDialogue, 0);
    }

    public void ShowDialogue(DialogueObject dialogueObject, int dialogueNumber)
    {
        dialogueBox.SetActive(true);
        //Time.timeScale = 0f;
        StartCoroutine(StepThroughDialogue(dialogueObject, dialogueNumber));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject, int dialogueNumber)
    {
        dialogueBoxisOpen = true;
        string dialogue = dialogueObject.Dialogue[dialogueNumber];
        yield return typewritingEffect.Run(dialogue, textLabel);
        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        dialogueBoxisOpen = false;
        CloseDialogueBox();

        /*
        for(int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i];
            yield return typewritingEffect.Run(dialogue, textLabel);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }
        */
    }

    private void CloseDialogueBox()
    {
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
        //Time.timeScale = 1f;
    }
}
