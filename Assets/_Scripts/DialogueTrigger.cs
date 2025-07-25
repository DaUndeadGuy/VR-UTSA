
using UnityEngine;
using UnityEngine.Events;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    // --- NEW: References to the UI components in the scene ---
    public DialogueUI npcDialogueUI;
    public DialogueUI playerDialogueUI;

    // --- NEW: Events are now here, in the scene-based component ---
    public UnityEvent onDialogueStart;
    public UnityEvent onDialogueEnd;

    public bool startOnSceneStart = false;

    void Start()
    {
        // If the flag is set, trigger the dialogue immediately
        if (startOnSceneStart)
        {
            TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
        // Check if a dialogue is already running before starting a new one
        if (DialogueManagerV2.instance.IsDialogueRunning)
        {
            return; // Do nothing if a dialogue is already active
        }

        // --- UPDATED: Pass this entire trigger component to the manager ---
        DialogueManagerV2.instance.StartDialogue(this);
    }

    // Example of how to trigger dialogue manually
    private void OnMouseDown()
    {
        if (!startOnSceneStart)
        {
            TriggerDialogue();
        }
    }
}