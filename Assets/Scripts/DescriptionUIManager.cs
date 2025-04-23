using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionUIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI commonNameText;
    [SerializeField] private TextMeshProUGUI scientificNameText;
    [SerializeField] private TextMeshProUGUI genusText;
    [SerializeField] private TextMeshProUGUI familyText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI habitatText;
    [SerializeField] private TextMeshProUGUI factsText;
    [SerializeField] private TextMeshProUGUI xpText;
    
    [SerializeField] private TextMeshProUGUI matchPercentagesText;
    [SerializeField] private TextMeshProUGUI matchWithText;
    
    [SerializeField] private Button continueButton;

    [SerializeField] private PlanetNetAPIManager planetNetAPIManager;
    [SerializeField] private TogetherAIPlantInfo aiAPIManager;

    private int xpForChallenege = 70;
    private int currentChallenge = 0;
    

    private void Start()
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
        aiAPIManager.ReceivedTogetherResponse += OnReceivedDescription;
        XPManager.Instance.XPAdded += OnXPAdded;
    }
    
    private void UnSubscribe()
    {
        planetNetAPIManager.ReceivedPlanetNetResponse -= OnReceivedApiResponse;
        aiAPIManager.ReceivedTogetherResponse -= OnReceivedDescription;
        XPManager.Instance.XPAdded -= OnXPAdded;
    }

    public void InitDescription()
    {
        currentChallenge = 0;
        
        matchPercentagesText.text = "0% Match";
        matchWithText.text = "";
        
        commonNameText.text = "";
        scientificNameText.text = "";
        genusText.text = "";
        familyText.text = "";
        descriptionText.text = "";
        
        descriptionText.text = "";
        habitatText.text = "";
        factsText.text = "";

        continueButton.interactable = false;
    }
    
    private void OnXPAdded(int value)
    {
        currentChallenge += value;
        xpText.text = $"{currentChallenge}<size=30>XP</size>";
    }

    private void OnReceivedApiResponse(APIResponse response)
    {
        if(response == null || response.results == null || response.results.Count == 0)
        {
            HandleNoMatchFound(response);
            return;
        }

        //ShowXP();
        XPManager.Instance.AddXP(xpForChallenege);
        
        matchPercentagesText.text = $"{(int)(response.results[0].score*100)}% Match";
        matchWithText.text = $"Match with <b>{response.results[0].species.commonNames[0]}</b>";
        
        //StringBuilder commonNames = new StringBuilder();
        string commonNames = response.results[0].species.commonNames[0];
        // foreach (string commonName in response.results[0].species.commonNames)
        // {
        //     commonNames.Append($" {commonName},");
        // }

        // if (commonNames.Length > 0)
        //     commonNames.Length--;
        
        commonNameText.text = $"{commonNames}";
        scientificNameText.text = $"{response.results[0].species.scientificName}";
        genusText.text = $"{response.results[0].species.genus.scientificName}";
        familyText.text = $"{response.results[0].species.family.scientificName}";
        descriptionText.text = "";
    }

    private void HandleNoMatchFound(APIResponse response)
    {
        matchPercentagesText.text = "0% Match";
        matchWithText.text = $"{response.message}";
        scientificNameText.text = "Try again with better photo";
    }
    
    private void OnReceivedDescription(PlantInfo plantInfo)
    {
        if(plantInfo == null || plantInfo.ToString() == "{}" || plantInfo.Description == "") return;
        
        StringBuilder facts = new StringBuilder();
        int count = 1;
        
        foreach (string fact in plantInfo.InterestingFacts)
        {
            facts.Append($"{count}. {fact}\n");
            count++;
        }
        descriptionText.text = plantInfo.Description;
        habitatText.text = plantInfo.Habitat;
        factsText.text = facts.ToString();
        
        continueButton.interactable = true;
    }
}
