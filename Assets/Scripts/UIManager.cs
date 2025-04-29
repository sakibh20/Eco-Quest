using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private TextMeshProUGUI xpInGameText;

    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField userNameInputField;
    [SerializeField] private Button submitButton;

    [SerializeField] private Canvas scanCanvas;
    [SerializeField] private GameObject gameAssets;
    
    [SerializeField] private TextMeshProUGUI userNameInProfileText;
    [SerializeField] private TextMeshProUGUI nameTextInProfile;
    [SerializeField] private TextMeshProUGUI xpTextInProfile;

    [SerializeField] private DescriptionUIManager descriptionUIManager;
    [SerializeField] private RewardUiManager rewardUiManager;

    private string nameValueKey = "playerName";
    private string userNameValueKey = "userName";

    public static UIManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void OnEnable()
    {
        XPManager.Instance.XPCountChanged += OnXPUpdated;
        
        nameInputField.onValueChanged.AddListener(ValidateLoginInput);
        userNameInputField.onValueChanged.AddListener(ValidateLoginInput);
    }

    private void OnDisable()
    {
        XPManager.Instance.XPCountChanged -= OnXPUpdated;
        
        nameInputField.onValueChanged.RemoveListener(ValidateLoginInput);
        userNameInputField.onValueChanged.RemoveListener(ValidateLoginInput);
    }

    private void Start()
    {
        submitButton.interactable = false;
        InitUi();
    }

    public void LogOut()
    {
        HideHome();
        HideProfileView();
        ShowSignUp();
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

    private void ValidateLoginInput(string value)
    {
        if (string.IsNullOrWhiteSpace(nameInputField.text) || string.IsNullOrWhiteSpace(userNameInputField.text))
        {
            submitButton.interactable = false;
        }

        else
        {
            submitButton.interactable = true;
        }
    }
    
    private void OnXPUpdated(int xp)
    {
        xpText.text = $"{XPManager.Instance.XP.ToString()} XP";
        xpInGameText.text = $"{XPManager.Instance.XP.ToString()}";
        xpTextInProfile.text = $"XP: {XPManager.Instance.XP.ToString()}";
        
        GameDataManager.Instance.playerData.xp = XPManager.Instance.XP;
        GameDataManager.Instance.SavePlayer();
    }

    
    private void UpdateStoredInfo()
    {
        PlayerPrefs.SetString(nameValueKey, nameInputField.text);
        PlayerPrefs.SetString(userNameValueKey, userNameInputField.text);

        GameDataManager.Instance.playerData.name = nameInputField.text;
        GameDataManager.Instance.playerData.userName = userNameInputField.text;
        
        GameDataManager.Instance.SavePlayer();
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

    private void HideHome()
    {
        homePanel.SetActive(false);
    }
    
    private void ShowHome()
    {
        profileName.text = $"Hello, {PlayerPrefs.GetString(userNameValueKey)}";
        inputPanel.SetActive(false);
        homePanel.SetActive(true);

        nameTextInProfile.text = $"Name: {PlayerPrefs.GetString(nameValueKey)}";
        userNameInProfileText.text = $"Username: {PlayerPrefs.GetString(userNameValueKey)}";
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
        HideChallengesView();
        HideCollectionsView();
        profilePanel.SetActive(true);
    }
    
    public void HideProfileView()
    {
        profilePanel.SetActive(false);
    }
    
    public void ShowChallengesView()
    {
        HideProfileView();
        HideCollectionsView();
        challengesPanel.SetActive(true);
        //rewardUiManager.Init();
    }
    
    public void HideChallengesView()
    {
        challengesPanel.SetActive(false);
    }
    
    public void ShowCollectionsView()
    {
        HideProfileView();
        HideChallengesView();
        collectionPanel.SetActive(true);
        //rewardUiManager.Init();
    }
    
    public void HideCollectionsView()
    {
        collectionPanel.SetActive(false);
    }
    
    public void ShowRewardView()
    {
        rewardPanel.SetActive(true);
        //rewardUiManager.Init();
    }
    
    public void HideRewardView()
    {
        rewardPanel.SetActive(false);
    }

    public void ShowGame()
    {
        gameAssets.SetActive(true);
        scanCanvas.gameObject.SetActive(false);
    }

    public void HideGame()
    {
        gameAssets.SetActive(false);
        scanCanvas.gameObject.SetActive(true);
    }
}
