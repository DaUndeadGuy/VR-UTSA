// 25/07/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEditor;
using UnityEngine;

public class DialogueUIManager : MonoBehaviour
{
    public GameObject defaultDialogueUI; // Default UI prefab
    public GameObject character1DialogueUI; // Character 1's UI prefab
    public GameObject character2DialogueUI; // Character 2's UI prefab

    private GameObject currentDialogueUI;

    public void ShowDialogue(string characterName, string dialogueText)
    {
        // Destroy the current UI if it exists
        if (currentDialogueUI != null)
        {
            Destroy(currentDialogueUI);
        }

        // Choose the appropriate UI prefab based on the character
        switch (characterName)
        {
            case "Character1":
                currentDialogueUI = Instantiate(character1DialogueUI, transform);
                break;
            case "Character2":
                currentDialogueUI = Instantiate(character2DialogueUI, transform);
                break;
            default:
                currentDialogueUI = Instantiate(defaultDialogueUI, transform);
                break;
        }

        // Set the dialogue text (assuming the prefab has a Text component)
        var dialogueTextComponent = currentDialogueUI.GetComponentInChildren<UnityEngine.UI.Text>();
        if (dialogueTextComponent != null)
        {
            dialogueTextComponent.text = dialogueText;
        }
    }
}
