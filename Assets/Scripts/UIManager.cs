using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject inputPanel;
    [SerializeField] private GameObject profilePanel;
    [SerializeField] private TextMeshProUGUI profileName;

    [SerializeField] private TMP_InputField nameInputField;
    
    public void Submit()
    {
        inputPanel.SetActive(false);
        profilePanel.SetActive(true);
        //TODO: Text Set
        profileName.text= nameInputField.text;
    }
}
