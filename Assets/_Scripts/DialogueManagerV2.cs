
using UnityEngine;
using UnityEngine.Playables; // Required for Timeline functionality
using System.Collections;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(PlayableDirector))] // Enforce that a PlayableDirector exists
public class DialogueManagerV2 : MonoBehaviour
{
    public static DialogueManagerV2 instance;

    private DialogueTrigger currentTrigger;
    private AudioSource audioSource;
    private PlayableDirector playableDirector; // --- NEW: To play Timelines

    public float autoAdvanceDelay = 2.0f;
    public float typingSpeed = 0.04f;

    public bool IsDialogueRunning { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
        playableDirector = GetComponent<PlayableDirector>(); // --- NEW: Get the director
        IsDialogueRunning = false;
    }

    public void StartDialogue(DialogueTrigger trigger)
    {
        if (trigger == null || trigger.dialogue == null)
        {
            Debug.LogError("StartDialogue was called with an invalid trigger or the trigger has no Dialogue asset assigned.");
            return;
        }

        IsDialogueRunning = true;
        this.currentTrigger = trigger;

        if (currentTrigger.onDialogueStart != null)
        {
            Debug.Log($"Dialogue '{currentTrigger.dialogue.npcName}' starting. Invoking onDialogueStart event from trigger.");
            currentTrigger.onDialogueStart.Invoke();
        }

        StopAllCoroutines();
        StartCoroutine(RunDialogueFlow());
    }

    private IEnumerator RunDialogueFlow()
    {
        foreach (DialogueLine line in currentTrigger.dialogue.lines)
        {
            DialogueUI uiToShow = (line.speaker == Speaker.NPC) ? currentTrigger.npcDialogueUI : currentTrigger.playerDialogueUI;
            DialogueUI uiToClear = (line.speaker == Speaker.NPC) ? currentTrigger.playerDialogueUI : currentTrigger.npcDialogueUI;

            if (uiToShow == null)
            {
                Debug.LogError($"Dialogue UI for speaker '{line.speaker}' is not assigned in the DialogueTrigger. Please drag the component from your scene into the Inspector.", this);
                yield break;
            }

            if (uiToClear != null)
            {
                uiToClear.ClearText();
            }

            // --- NEW: Play the Timeline clip if one is assigned ---
            if (line.timelineClip != null)
            {
                playableDirector.playableAsset = line.timelineClip;
                playableDirector.Play();
            }

            if (line.audioClip != null)
            {
                audioSource.Stop();
                audioSource.PlayOneShot(line.audioClip);
            }

            yield return StartCoroutine(uiToShow.TypeSentence(line.sentence, typingSpeed));

            float delay = line.customDelay > 0 ? line.customDelay : autoAdvanceDelay;
            yield return new WaitForSeconds(delay);
        }

        EndDialogue();
    }

    void EndDialogue()
    {
        IsDialogueRunning = false;

        if (currentTrigger != null)
        {
            if (currentTrigger.npcDialogueUI != null) currentTrigger.npcDialogueUI.ClearText();
            if (currentTrigger.playerDialogueUI != null) currentTrigger.playerDialogueUI.ClearText();

            if (currentTrigger.onDialogueEnd != null)
            {
                Debug.Log($"Dialogue '{currentTrigger.dialogue.npcName}' ended. Invoking onDialogueEnd event from trigger.");
                currentTrigger.onDialogueEnd.Invoke();
            }
        }
    }
}