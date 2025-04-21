using System;
using UnityEngine;

public class XPManager : MonoBehaviour
{
    private int xp;
    public int XP => xp;

    private string xpKey = "xp";

    public event Action<int> XPAdded; 
    public event Action<int> XPCountChanged; 
    
    public static XPManager Instance;

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

    private void Start()
    {
        InitXP();
    }

    private void InitXP()
    {
        if (PlayerPrefs.HasKey(xpKey))
        {
            xp = PlayerPrefs.GetInt(xpKey);
            StoreXP();
        }
    }

    public void AddXP(int value)
    {
        xp += value;
        StoreXP();
        XPAdded?.Invoke(value);
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
        
        XPCountChanged?.Invoke(xp);
    }
}
