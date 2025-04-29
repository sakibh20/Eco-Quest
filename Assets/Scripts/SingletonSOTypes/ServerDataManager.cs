using System;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

[Serializable]
[CreateAssetMenu(menuName = "SingletonSOs/GameDataManager")]
public class GameDataManager : SingletonSO<GameDataManager>
{
    public PlayerData playerData;
    public ChallengeData challengeData;
    public CollectionData collectionData;

    private static string basePath => Application.persistentDataPath;
    private static string PlayerPath => Path.Combine(basePath, "PlayerData.json");
    private static string CollectionPath => Path.Combine(basePath, "Collections.json");
    private static string ChallengePath => Path.Combine(basePath, "Challenges.json");

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void OnBeforeSceneLoad()
    {
        GameDataManager instance = Instance;
        if (instance != null)
        {
            instance.LoadPlayer();
            instance.LoadCollections();
            instance.LoadChallenges();

            Debug.Log("ServerDataManager loaded successfully.");
        }
        else
        {
            Debug.LogError("ServerDataManager instance is null.");
        }
    }

    public void LoadAll()
    {
        LoadPlayer();
        LoadCollections();
        LoadChallenges();
    }

    public void SavePlayer()
    {
        File.WriteAllText(PlayerPath, JsonUtility.ToJson(playerData, true));
    }

    public void LoadPlayer() {
        if (!File.Exists(PlayerPath)) {
            SavePlayer();
        }
        playerData = JsonUtility.FromJson<PlayerData>(File.ReadAllText(PlayerPath));
    }

    [ContextMenu("SaveCollections")]
    public void SaveCollections()
    {
        File.WriteAllText(CollectionPath, JsonUtility.ToJson(collectionData, true));
    }

    public void LoadCollections() {
        if (!File.Exists(CollectionPath)) {
            SaveCollections();
        }
        collectionData =  JsonUtility.FromJson<CollectionData>(File.ReadAllText(CollectionPath));
    }

    [ContextMenu("SaveChallenges")]
    public void SaveChallenges()
    {
        File.WriteAllText(ChallengePath, JsonUtility.ToJson(challengeData, true));
    }

    public void LoadChallenges() {
        if (!File.Exists(ChallengePath)) {
            SaveChallenges();
        }
        challengeData = JsonUtility.FromJson<ChallengeData>(File.ReadAllText(ChallengePath));
    }
}

[Serializable]
public class PlayerData
{
    public string name;
    public string userName;
    public int xp;
    public int level;
}

[Serializable]
public class CollectionData
{
    public string userName;
    public List<Species> foundSpecies = new();
}

[Serializable]
public class Challenge
{
    public string title;
    public string description;

    public ChallengeButtonMode type;
    public bool completed;
}

[Serializable]
public class ChallengeData
{
    public string userName;
    public List<Challenge> challenges = new();
}
