using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private DialogueObject textDialogue;

    private TypewritingEffect typewritingEffect;

    private void Start()
    {
        typewritingEffect = GetComponent<TypewritingEffect>();
        //CloseDialogueBox();
        ShowDialogue(textDialogue);
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        //dialogueBox.SetActive(true);
        //Time.timeScale = 0f;
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        for(int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i];
            yield return typewritingEffect.Run(dialogue, textLabel);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }

        CloseDialogueBox();
    }

    private void CloseDialogueBox()
    {
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
        //Time.timeScale = 1f;
    }
}
