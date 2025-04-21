using UnityEngine;

public class LoadingUiManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingPanel;
    
    public static LoadingUiManager Instance;

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

    public void ShowLoadingPanel()
    {
        loadingPanel.SetActive(true);
    }

    public void HideLoadingPanel()
    {
        loadingPanel.SetActive(false);
    }
}
