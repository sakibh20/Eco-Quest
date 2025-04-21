using System.Collections.Generic;
using UnityEngine;

public class RewardUiManager : MonoBehaviour
{
    [SerializeField] private Transform itemParent;
    [SerializeField] private QuestionItem questionItemPrefab;
    [SerializeField] private TogetherAIPlantInfo aiAPIManager;

    private List<QuestionItem> _allGeneratedItems = new List<QuestionItem>();
    
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
    
    public void Init()
    {
        foreach (QuestionItem item in _allGeneratedItems)
        {
            Destroy(item.gameObject);
        }

        _allGeneratedItems = new List<QuestionItem>();
    }
    
    private void OnReceivedDescription(PlantInfo plantInfo)
    {
        Init();
        
        if(plantInfo == null || plantInfo.ToString() == "{}" || plantInfo.Description == "") return;

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
    }
}
