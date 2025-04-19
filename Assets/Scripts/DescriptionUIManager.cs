using System;
using System.Text;
using TMPro;
using UnityEngine;

public class DescriptionUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI commonNameText;
    [SerializeField] private TextMeshProUGUI scientificNameText;
    [SerializeField] private TextMeshProUGUI genusText;
    [SerializeField] private TextMeshProUGUI familyText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    
    [SerializeField] private TextMeshProUGUI matchPercentagesText;
    [SerializeField] private TextMeshProUGUI matchWithText;

    [SerializeField] private PlanetNetAPIManager planetNetAPIManager;
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
        planetNetAPIManager.ReceivedPlanetNetResponse += OnReceivedApiResponse;
    }
    
    private void UnSubscribe()
    {
        planetNetAPIManager.ReceivedPlanetNetResponse -= OnReceivedApiResponse;
    }

    public void InitDescription()
    {
        matchPercentagesText.text = "0%";
        matchWithText.text = "";
        
        commonNameText.text = " -";
        scientificNameText.text = " -";
        genusText.text = " -";
        familyText.text = " -";
        descriptionText.text = "";
    }

    private void OnReceivedApiResponse(APIResponse response)
    {
        if(response.results.Count == 0)
        {
            matchPercentagesText.text = "0%";
            matchWithText.text = "No Match Found";
            
            return;
        }
        
        matchPercentagesText.text = $"{(int)(response.results[0].score*100)}%";
        matchWithText.text = $"Match with <b>{response.results[0].species.commonNames[0]}</b>";
        
        StringBuilder commonNames = new StringBuilder();
        foreach (string commonName in response.results[0].species.commonNames)
        {
            commonNames.Append($" {commonName},");
        }

        if (commonNames.Length > 0)
            commonNames.Length--;
        
        commonNameText.text = $"{commonNames}";
        scientificNameText.text = $"{response.results[0].species.scientificName}";
        genusText.text = $"{response.results[0].species.genus.scientificName}";
        familyText.text = $"{response.results[0].species.family.scientificName}";
        descriptionText.text = "";
    }
}
