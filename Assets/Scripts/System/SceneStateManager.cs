
using System.Collections.Generic;
using UnityEngine;

public class SceneStateManager : MonoBehaviour
{
    public static SceneStateManager Instance { get; private set; }

    private Dictionary<string, bool> states = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Duplicate SceneStateManager!");
            Debug.LogWarning("Duplicate destroyed!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private string MakeKey(
        string sceneName,
        string objectId,
        string stateName)
    {
        return sceneName + "/" + objectId + "/" + stateName;
    }

    public void SetBool(
        string sceneName,
        string objectId,
        string stateName,
        bool value)
    {
        string key = MakeKey(sceneName, objectId, stateName);
        states[key] = value;
    }

    public bool GetBool(
        string sceneName,
        string objectId,
        string stateName)
    {
        string key = MakeKey(sceneName, objectId, stateName);

        return states.TryGetValue(key, out bool value) && value;
    }
}
