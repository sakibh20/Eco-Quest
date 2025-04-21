using UnityEngine;

public class XPManager : MonoBehaviour
{
    private int xp;
    public int XP => xp;

    private string xpKey = "xp";
    
    public static XPManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitXP();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void InitXP()
    {
        if (PlayerPrefs.HasKey(xpKey))
        {
            xp = PlayerPrefs.GetInt(xpKey);
        }
    }

    public void AddXP(int value)
    {
        xp += value;
        StoreXP();
    }

    public void ReduceXP(int value)
    {
        if(!HasEnoughXP(value)) return;
        xp -= value;
        StoreXP();
    }

    public bool HasEnoughXP(int cost)
    {
        return (xp - cost) >= 0;
    }

    private void StoreXP()
    {
        PlayerPrefs.SetInt(xpKey, xp);
    }
}
