using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RewardUiManager : MonoBehaviour
{
    [SerializeField] private Transform itemParent;
    [SerializeField] private QuestionItem questionItemPrefab;
    [SerializeField] private TogetherAIPlantInfo aiAPIManager;
    
    [SerializeField] private Button submitButton;

    private List<QuestionItem> _allGeneratedItems = new List<QuestionItem>();

    private void Awake()
    {
        _allGeneratedItems = new List<QuestionItem>();
    }

    private void OnEnable()
    {
        Subscribe();
    }

    private void OnDestroy()
    {
        UnSubscribe();
    }

    private void Subscribe()
    {
        aiAPIManager.ReceivedTogetherResponse += OnReceivedDescription;
    }
    
    private void UnSubscribe()
    {
        aiAPIManager.ReceivedTogetherResponse -= OnReceivedDescription;
    }
    
    private void Init()
    {
        foreach (QuestionItem item in _allGeneratedItems)
        {
            Destroy(item.gameObject);
        }

        _allGeneratedItems = new List<QuestionItem>();

        submitButton.interactable = true;
    }
    
    private void OnReceivedDescription(PlantInfo plantInfo)
    {
        Init();
        
        if(plantInfo.ToString() == "{}" || plantInfo.Description == "") return;

        Debug.Log($"plantInfo: {plantInfo}");
        Debug.Log($"plantInfo.Description: {plantInfo.Description}");
        
        foreach (MCQ mcq in plantInfo.MCQs)
        {
            QuestionItem item = Instantiate(questionItemPrefab, itemParent);
            item.SetMCQ(mcq);
            _allGeneratedItems.Add(item);
        }
    }

    public void GetResult()
    {
        int correctCount = 0;
        
        foreach (QuestionItem questionItem in _allGeneratedItems)
        {
            if (questionItem.IsCorrect()) correctCount += 1;
        }

        int additionalReward = correctCount * 5;
        
        Debug.Log($"AdditionalReward: {additionalReward}");
        
        XPManager.Instance.AddXP(additionalReward);
        
        submitButton.interactable = false;
    }
}
