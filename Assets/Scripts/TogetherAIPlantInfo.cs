using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class TogetherAIPlantInfo : MonoBehaviour
{
    [Header("Together AI Settings")]
    [SerializeField] private string togetherAPIKey;
    [SerializeField] private string model = "mistralai/Mixtral-8x7B-Instruct-v0.1";

    public PlantInfo info2;
    public PlantInfo info;
    [SerializeField] private PlanetNetAPIManager planetNetAPIManager;

    private int _tryCount = 0;

    private APIResponse _lastResponse;
    public event Action<PlantInfo> ReceivedTogetherResponse;

    private void OnEnable() => planetNetAPIManager.ReceivedPlanetNetResponse += OnReceivedApiResponse;
    private void OnDestroy() => planetNetAPIManager.ReceivedPlanetNetResponse -= OnReceivedApiResponse;

    private void OnReceivedApiResponse(APIResponse response)
    {
        if (response.results.Count == 0) return;

        _lastResponse = response;
        _tryCount = 0;

        GetDescription();
    }

    private void GetDescription()
    {
        _tryCount += 1;
        
        if(_tryCount == 5) return;
        
        RequestPlantInfo(
            _lastResponse.results[0].species.commonNames[0],
            _lastResponse.results[0].species.family.scientificName
        );
    }

    private void RequestPlantInfo(string commonName, string scientificName)
    {
        string prompt = $"Only return a clean JSON. Structure: {{\"Description\":\"...\",\"Habitat\":\"...\",\"Interesting_Facts\":[\"...\"],\"MCQs\":[{{\"question\":\"...\",\"options\":[\"A\",\"B\",\"C\",\"D\"],\"answer\":\"A\"}}]}} " +
                        $"Info: Common Name = {commonName}, Scientific Name = {scientificName}";

        StartCoroutine(CallTogetherAI(prompt));
    }

    private IEnumerator CallTogetherAI(string inputText)
    {
        string url = "https://api.together.xyz/v1/chat/completions";

        var payload = new TogetherRequest
        {
            model = model,
            messages = new List<Message>
            {
                new Message { role = "user", content = inputText }
            },
            temperature = 0.7f,
            max_tokens = 512
        };

        string jsonData = JsonConvert.SerializeObject(payload);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Authorization", $"Bearer {togetherAPIKey}");
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        info = new PlantInfo();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("OpenAI API Error: " + request.error);
            Debug.LogError("Response: " + request.downloadHandler.text);
        }
        else
        {
            string responseText = request.downloadHandler.text;
            Debug.Log("Together.ai Response: " + responseText);

            try
            {
                var togetherResponse = JsonConvert.DeserializeObject<TogetherResponse>(responseText);
                string output = togetherResponse.choices[0].message.content;

                string extractedJson = ExtractFirstJsonBlock(output);
                if (!string.IsNullOrEmpty(extractedJson))
                {
                    info = JsonConvert.DeserializeObject<PlantInfo>(extractedJson);
                    Debug.Log("Parsed Plant Info!");
                }
                else
                {
                    Debug.LogWarning("Could not extract JSON from model output.");
                    GetDescription();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Parsing Error: " + ex.Message);
                GetDescription();
            }
        }

        ReceivedTogetherResponse?.Invoke(info);
    }

    private string ExtractFirstJsonBlock(string input)
    {
        int firstBrace = input.IndexOf('{');
        int lastBrace = input.LastIndexOf('}');
        if (firstBrace >= 0 && lastBrace > firstBrace)
            return input.Substring(firstBrace, lastBrace - firstBrace + 1);
        return null;
    }
}

[Serializable]
public class TogetherRequest
{
    public string model;
    public List<Message> messages;
    public float temperature;
    public int max_tokens;
}

[Serializable]
public class Message
{
    public string role;
    public string content;
}

[Serializable]
public class TogetherResponse
{
    public List<Choice> choices;
}

[Serializable]
public class Choice
{
    public Message message;
}

[Serializable]
public class PlantInfo
{
    public string Description;
    public string Habitat;
    [JsonProperty("Interesting_Facts")]
    public List<string> InterestingFacts;
    public List<MCQ> MCQs;
}

[Serializable]
public class MCQ
{
    public string question;
    public List<string> options;
    public string answer;
}
