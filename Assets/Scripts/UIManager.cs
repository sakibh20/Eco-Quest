using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject inputPanel;
    [SerializeField] private GameObject profilePanel;
    [SerializeField] private GameObject descriptionPanel;
    [SerializeField] private TextMeshProUGUI profileName;

    [SerializeField] private TMP_InputField nameInputField;

    [SerializeField] private DescriptionUIManager descriptionUIManager;
    
    public void Submit()
    {
        inputPanel.SetActive(false);
        profilePanel.SetActive(true);
        profileName.text = $"Hello, {nameInputField.text}";
    }

    public void ShowDescriptionView()
    {
        descriptionUIManager.InitDescription();
        descriptionPanel.SetActive(true);
    }
}
