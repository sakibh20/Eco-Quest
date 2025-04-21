using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeButton : MonoBehaviour
{
    [SerializeField] private ChallengeButtonMode mode;

    [SerializeField] private Image buttonImage;
    [SerializeField] private TextMeshProUGUI buttonText;
    
    [SerializeField] private Color32 playColor;
    [SerializeField] private Color32 scanColor;

    private Button _button;

    private CameraManager _cameraManager;

    private void Awake()
    {
        _cameraManager = FindAnyObjectByType<CameraManager>();
        _button = GetComponent<Button>();
        InItButton();
    }

    private void InItButton()
    {
        buttonImage.color = mode == ChallengeButtonMode.Scan ? scanColor : playColor;
        buttonText.text = mode == ChallengeButtonMode.Scan ? "Scan" : "Play";
    }

    private void Start()
    {
        _button.onClick.AddListener(OnClickButton);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(OnClickButton);
    }

    private void OnClickButton()
    {
        if (mode == ChallengeButtonMode.Play)
        {
            UIManager.Instance.ShowGame();
        }
        else
        {
            _cameraManager.CapturePhoto();
            UIManager.Instance.ShowPreview();
        }
    }
}

public enum ChallengeButtonMode
{
    Scan,
    Play
}
