using System;
using UnityEngine;

public class PersistentEntity : MonoBehaviour
{
    [SerializeField] private string entityId;

    public string EntityId => entityId;
    public bool IsRuntimeSpawned { get; private set; }
    public GameObject SourcePrefab { get; private set; }

    // Dùng cho entity được tạo trong lúc chơi
    public void Initialize(
        string id,
        GameObject sourcePrefab,
        bool runtimeSpawned)
    {
        entityId = id;
        SourcePrefab = sourcePrefab;
        IsRuntimeSpawned = runtimeSpawned;
    }

    // Dùng để tạo ID cho entity đặt sẵn trong Scene
    [ContextMenu("Generate New Entity ID")]
    public void GenerateNewEntityId()
    {
        entityId = Guid.NewGuid().ToString("N");
        Debug.Log("Generated Entity ID: " + entityId, this);
    }
}

