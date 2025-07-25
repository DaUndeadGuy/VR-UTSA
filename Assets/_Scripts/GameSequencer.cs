
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public enum SequenceActionType
{
    StartDialogue,
    FadeIn,
    FadeOut,
    Wait,
    InvokeEvent
}

[System.Serializable]
public class SequenceStep
{
    public string stepName; // For readability in the inspector
    public SequenceActionType action;

    // Fields for the different action types
    public DialogueTrigger dialogueTrigger;
    public FadeScreen fadeScreen;
    public float waitDuration;
    public UnityEvent eventToInvoke;
}

public class GameSequencer : MonoBehaviour
{
    public List<SequenceStep> sequenceSteps;
    public bool startOnAwake = true; // If false, the sequence must be started by calling BeginSequence()
    private int currentStepIndex = 0;

    void Start()
    {
        if (startOnAwake)
        {
            BeginSequence();
        }
    }

    public void BeginSequence()
    {
        if (sequenceSteps != null && sequenceSteps.Count > 0)
        {
            currentStepIndex = 0;
            ExecuteStep(currentStepIndex);
        }
    }


    private void ExecuteStep(int index)
    {
        if (index >= sequenceSteps.Count)
        {
            Debug.Log("Game Sequence Completed.");
            return;
        }

        SequenceStep currentStep = sequenceSteps[index];
        Debug.Log($"Executing Sequence Step {index}: {currentStep.stepName} ({currentStep.action})");

        switch (currentStep.action)
        {
            case SequenceActionType.StartDialogue:
                if (currentStep.dialogueTrigger != null)
                {
                    // Listen to the event on the trigger itself
                    currentStep.dialogueTrigger.onDialogueEnd.AddListener(OnCurrentStepCompleted);
                    currentStep.dialogueTrigger.TriggerDialogue();
                }
                else
                {
                    Debug.LogError($"Sequence Step {index} is 'StartDialogue' but no DialogueTrigger is assigned.", this);
                    OnCurrentStepCompleted();
                }
                break;

            case SequenceActionType.FadeIn:
                if (currentStep.fadeScreen != null)
                {
                    currentStep.fadeScreen.onFadeInComplete.AddListener(OnCurrentStepCompleted);
                    currentStep.fadeScreen.BeginFadeIn();
                }
                else
                {
                    Debug.LogError($"Sequence Step {index} is 'FadeIn' but no FadeScreen is assigned.", this);
                    OnCurrentStepCompleted();
                }
                break;

            case SequenceActionType.FadeOut:
                if (currentStep.fadeScreen != null)
                {
                    currentStep.fadeScreen.onFadeOutComplete.AddListener(OnCurrentStepCompleted);
                    currentStep.fadeScreen.BeginFadeOut();
                }
                else
                {
                    Debug.LogError($"Sequence Step {index} is 'FadeOut' but no FadeScreen is assigned.", this);
                    OnCurrentStepCompleted();
                }
                break;

            case SequenceActionType.Wait:
                StartCoroutine(WaitCoroutine(currentStep.waitDuration));
                break;

            case SequenceActionType.InvokeEvent:
                if (currentStep.eventToInvoke != null)
                {
                    currentStep.eventToInvoke.Invoke();
                }
                OnCurrentStepCompleted(); // Events are considered instant
                break;
        }
    }

    private IEnumerator WaitCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        OnCurrentStepCompleted();
    }

    private void OnCurrentStepCompleted()
    {
        // Unsubscribe from the event to prevent it from being called again
        if (currentStepIndex >= sequenceSteps.Count) return;

        SequenceStep completedStep = sequenceSteps[currentStepIndex];
        switch (completedStep.action)
        {
            case SequenceActionType.StartDialogue:
                if (completedStep.dialogueTrigger != null)
                    // Unsubscribe from the event on the trigger
                    completedStep.dialogueTrigger.onDialogueEnd.RemoveListener(OnCurrentStepCompleted);
                break;
            case SequenceActionType.FadeIn:
                if (completedStep.fadeScreen != null)
                    completedStep.fadeScreen.onFadeInComplete.RemoveListener(OnCurrentStepCompleted);
                break;
            case SequenceActionType.FadeOut:
                if (completedStep.fadeScreen != null)
                    completedStep.fadeScreen.onFadeOutComplete.RemoveListener(OnCurrentStepCompleted);
                break;
        }

        currentStepIndex++;
        ExecuteStep(currentStepIndex);
    }
}
