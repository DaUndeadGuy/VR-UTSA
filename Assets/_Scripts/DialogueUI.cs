using UnityEngine;
using TMPro; // Import the TextMeshPro namespace
using System.Collections;

public class DialogueUI : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;

    public IEnumerator TypeSentence(string sentence, float speed)
    {
        // --- ERROR CHECK: Ensure the dialogueText field is assigned ---
        if (dialogueText == null)
        {
            Debug.LogError($"'Dialogue Text' (TextMeshProUGUI) is not assigned on the DialogueUI component of '{gameObject.name}'. Please assign it in the Inspector.", gameObject);
            yield break; // Stop the coroutine
        }

        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(speed);
        }
    }

    // --- NEW: Method to clear the dialogue text ---
    public void ClearText()
    {
        if (dialogueText != null)
        {
            dialogueText.text = "";
        }
    }
}
