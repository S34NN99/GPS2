using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject extinguisher;
    [SerializeField] private GameObject medic;
    [SerializeField] private GameObject demolisher;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private DialogueObject textDialogue;

    public BoxCollider boxOne;
    public BoxCollider boxTwo;
    public BoxCollider boxThree;
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
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        foreach(string dialogue in dialogueObject.Dialogue)
        {
            yield return typewritingEffect.Run(dialogue, textLabel);
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        }

        CloseDialogueBox();

        
    }

    private void CloseDialogueBox()
    {
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }
}
