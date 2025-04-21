using System;
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
    [SerializeField] private TextMeshProUGUI xpInGameText;

    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField userNameInputField;

    [SerializeField] private Canvas scanCanvas;
    [SerializeField] private Canvas gameCanvas;

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
    }

    private void OnDisable()
    {
        XPManager.Instance.XPCountChanged -= OnXPUpdated;
    }

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
    
    private void OnXPUpdated(int xp)
    {
        xpText.text = $"{XPManager.Instance.XP.ToString()} XP";
        xpInGameText.text = $"{XPManager.Instance.XP.ToString()}";
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
        inputPanel.SetActive(false);
        homePanel.SetActive(true);
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
        //rewardUiManager.Init();
    }
    
    public void HideRewardView()
    {
        rewardPanel.SetActive(false);
    }

    public void ShowGame()
    {
        gameCanvas.gameObject.SetActive(true);
        scanCanvas.gameObject.SetActive(false);
    }

    public void HideGame()
    {
        gameCanvas.gameObject.SetActive(false);
        scanCanvas.gameObject.SetActive(true);
    }
}
