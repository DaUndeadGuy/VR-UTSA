// 26/07/2025 AI-Tag
// This was created with the help of Assistant, a Unity Artificial Intelligence product.

using System;
using UnityEditor;
using UnityEngine;

public class GameObjectSwitcher : MonoBehaviour
{
    // References to the GameObjects
    public GameObject objectToDisable;
    public GameObject objectToEnable;

    // Function to switch the GameObjects
    public void SwitchObjects()
    {
        if (objectToDisable != null)
        {
            objectToDisable.SetActive(false); // Disable the first GameObject
        }

        if (objectToEnable != null)
        {
            objectToEnable.SetActive(true); // Enable the second GameObject
        }
    }
}
