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
            var playerData = LoadPlayer();
            var collectionData = LoadCollections();
            var challengeData = LoadChallenges();

            instance.playerData = playerData;
            instance.collectionData = collectionData;
            instance.challengeData = challengeData;

            Debug.Log("ServerDataManager loaded successfully.");
        }
        else
        {
            Debug.LogError("ServerDataManager instance is null.");
        }
    }

    public void LoadAll()
    {
       playerData = LoadPlayer();
        collectionData = LoadCollections();
        challengeData = LoadChallenges();
    }

    public static void SavePlayer(PlayerData data)
    {
        File.WriteAllText(PlayerPath, JsonUtility.ToJson(data, true));
    }

    public static PlayerData LoadPlayer() {
        if (!File.Exists(PlayerPath)) {
            var empty = new PlayerData();
            SavePlayer(empty);
            return empty;
        }
        return JsonUtility.FromJson<PlayerData>(File.ReadAllText(PlayerPath));
    }

    public static void SaveCollections(CollectionData data)
    {
        File.WriteAllText(CollectionPath, JsonUtility.ToJson(data, true));
    }

    public static CollectionData LoadCollections() {
        if (!File.Exists(CollectionPath)) {
            var empty = new CollectionData { foundSpecies = new List<Species>() };
            SaveCollections(empty);
            return empty;
        }
        return JsonUtility.FromJson<CollectionData>(File.ReadAllText(CollectionPath));
    }

    public static void SaveChallenges(ChallengeData data)
    {
        File.WriteAllText(ChallengePath, JsonUtility.ToJson(data, true));
    }

    public static ChallengeData LoadChallenges() {
        if (!File.Exists(ChallengePath)) {
            var empty = new ChallengeData { challenges = new List<Challenge>() };
            SaveChallenges(empty);
            return empty;
        }
        return JsonUtility.FromJson<ChallengeData>(File.ReadAllText(ChallengePath));
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
    public List<string> description = new();

    public string type;
    public bool completed;
}

[Serializable]
public class ChallengeData
{
    public string userName;
    public List<Challenge> challenges = new();
}
