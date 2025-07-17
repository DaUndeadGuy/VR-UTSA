using UnityEngine;
using UnityEngine.UI;

public class LaptopDesktop : MonoBehaviour
{
    [SerializeField] private Button LiliButton;
    [SerializeField] private Button SamButton;

    [SerializeField] private Transform selectedTransform;

    [Header("Dialogue")]
    [SerializeField] private GameObject liliDialogueTrigger;
    [SerializeField] private GameObject SamDialogueTrigger;

    private bool isLiliSelected;
    private bool isSamSelected;

    private void Awake()
    {
        LiliButton.onClick.AddListener(() =>
        {
            if (isSamSelected)
            {
                LeanTween.scale(SamButton.gameObject, Vector3.one * 0.7f, .25f);
                SamButton.interactable = true;
            }
            LeanTween.scale(LiliButton.gameObject, Vector3.one, .25f);

            isLiliSelected = true;
            isSamSelected = false;

            liliDialogueTrigger.SetActive(true);
            LiliButton.interactable = false;
        });

        SamButton.onClick.AddListener(() =>
        {
            if (isLiliSelected)
            {
                LeanTween.scale(LiliButton.gameObject, Vector3.one * 0.7f, .25f);
                LiliButton.interactable = true;
            }
            LeanTween.scale(SamButton.gameObject, Vector3.one, .25f);

            isLiliSelected = false;
            isSamSelected = true;

            SamDialogueTrigger.SetActive(true);
            SamButton.interactable = false;
        });
    }
}
