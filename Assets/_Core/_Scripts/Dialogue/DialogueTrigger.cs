public class DialogueTrigger : InteractableDummy
{
    public Dialogue dialogue;

    //call interact method from InteractableDummy
    public override void Interact()
    {
        TriggerDialogue();
    }

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
