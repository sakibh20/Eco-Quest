using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject inputPanel;
    [SerializeField] private GameObject homePanel;
    [SerializeField] private GameObject previewPanel;
    [SerializeField] private GameObject descriptionPanel;
    [SerializeField] private GameObject rewardPanel;
    [SerializeField] private GameObject profilePanel;
    [SerializeField] private GameObject collectionPanel;
    [SerializeField] private GameObject challengesPanel;
    
    [SerializeField] private TextMeshProUGUI profileName;
    [SerializeField] private TextMeshProUGUI xpText;

    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField userNameInputField;

    [SerializeField] private DescriptionUIManager descriptionUIManager;

    private string nameValueKey = "playerName";
    private string userNameValueKey = "userName";

    private void Start()
    {
        InitUi();
    }

    private void InitUi()
    {
        if (PlayerPrefs.HasKey(nameValueKey))
        {
            ShowHome();
        }
        else
        {
            ShowSignUp();
        }
    }
    
    private void UpdateStoredInfo()
    {
        PlayerPrefs.SetString(nameValueKey, nameInputField.text);
        PlayerPrefs.SetString(userNameValueKey, userNameInputField.text);
    }

    public void OnClickSubmit()
    {
        UpdateStoredInfo();
        ShowHome();
    }

    private void ShowSignUp()
    {
        inputPanel.SetActive(true);
        homePanel.SetActive(false);
    }    
    
    private void ShowHome()
    {
        profileName.text = $"Hello, {PlayerPrefs.GetString(userNameValueKey)}";
        UpdateXPTextUi();
        inputPanel.SetActive(false);
        homePanel.SetActive(true);
    }

    public void UpdateXPTextUi()
    {
        xpText.text = $"{XPManager.Instance.XP.ToString()} XP";
    }
    
    public void ShowPreview()
    {
        previewPanel.SetActive(true);
    }
    
    public void HidePreview()
    {
        previewPanel.SetActive(false);
    }

    public void ShowDescriptionView()
    {
        descriptionUIManager.InitDescription();
        descriptionPanel.SetActive(true);
    }
    
    public void HideDescriptionView()
    {
        HidePreview();
        descriptionPanel.SetActive(false);
    }
    
    public void ShowProfileView()
    {
        profilePanel.SetActive(true);
    }
    
    public void HideProfileView()
    {
        profilePanel.SetActive(false);
    }
    
    public void ShowRewardView()
    {
        rewardPanel.SetActive(true);
    }
    
    public void HideRewardView()
    {
        rewardPanel.SetActive(false);
    }
}
