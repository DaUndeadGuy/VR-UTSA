
using UnityEngine;
using UnityEngine.Playables; // Required for PlayableAsset

// Defines who is speaking the line
public enum Speaker { NPC, PLAYER }

[System.Serializable]
public class DialogueLine
{
    public Speaker speaker;

    [TextArea(3, 10)]
    public string sentence;

    public float customDelay = 0f;

    public AudioClip audioClip;

    // --- NEW: Reference to a Timeline asset ---
    public PlayableAsset timelineClip;
}

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue System/Dialogue")]
public class Dialogue : ScriptableObject
{
    public string npcName;

    public DialogueLine[] lines;
}