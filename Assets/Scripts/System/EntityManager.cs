
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class EntityComponentSnapshot
{
    public string componentType;
    public string jsonData;
}

[Serializable]
public class EntityRecord
{
    public string entityId;
    public string sceneName;

    public bool isRuntimeSpawned;
    public bool isRemoved;
    public bool isActive;

    public Vector3 position;
    public Quaternion rotation;

    // Prefab nguồn, giữ trong bộ nhớ khi chuyển Scene
    public GameObject sourcePrefab;

    public List<EntityComponentSnapshot> componentStates = new();
}

public class EntityManager : MonoBehaviour
{
    public static EntityManager Instance { get; private set; }

    // Scene -> Entity ID -> Entity Data
    private Dictionary<string, Dictionary<string, EntityRecord>>
        sceneEntities = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        if (Instance == this)
            SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private Dictionary<string, EntityRecord> GetSceneRecords(
        string sceneName)
    {
        if (!sceneEntities.ContainsKey(sceneName))
        {
            sceneEntities[sceneName] =
                new Dictionary<string, EntityRecord>();
        }

        return sceneEntities[sceneName];
    }

    // =====================================
    // CAPTURE ENTITY
    // =====================================

    private EntityRecord CaptureEntity(PersistentEntity entity)
    {
        EntityRecord record = new EntityRecord();

        record.entityId = entity.EntityId;
        record.sceneName = entity.gameObject.scene.name;

        record.isRuntimeSpawned = entity.IsRuntimeSpawned;
        record.isActive = entity.gameObject.activeSelf;

        record.position = entity.transform.position;
        record.rotation = entity.transform.rotation;

        record.sourcePrefab = entity.SourcePrefab;

        MonoBehaviour[] components =
            entity.GetComponents<MonoBehaviour>();

        foreach (MonoBehaviour component in components)
        {
            if (component is not IEntityState saveable)
                continue;

            EntityComponentSnapshot snapshot =
                new EntityComponentSnapshot();

            snapshot.componentType =
                component.GetType().AssemblyQualifiedName;

            snapshot.jsonData = saveable.CaptureState();

            record.componentStates.Add(snapshot);
        }

        return record;
    }

    // =====================================
    // SAVE CURRENT SCENE
    // =====================================

    public void SaveCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        Dictionary<string, EntityRecord> records =
            GetSceneRecords(currentScene.name);

        PersistentEntity[] entities =
            FindObjectsByType<PersistentEntity>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        foreach (PersistentEntity entity in entities)
        {
            if (entity.gameObject.scene != currentScene)
                continue;

            if (string.IsNullOrEmpty(entity.EntityId))
            {
                Debug.LogError(
                    "Entity has no ID: " + entity.name,
                    entity
                );
                continue;
            }

            records[entity.EntityId] = CaptureEntity(entity);
        }

        Debug.Log(
            "Saved " + records.Count +
            " entities in Scene: " + currentScene.name
        );
    }

    // =====================================
    // SPAWN RUNTIME ENTITY
    // =====================================

    public GameObject SpawnEntity(
        GameObject prefab,
        Vector3 position,
        Quaternion rotation)
    {
        if (prefab == null)
        {
            Debug.LogError("Entity prefab is NULL!");
            return null;
        }

        string sceneName = SceneManager.GetActiveScene().name;
        string newId = Guid.NewGuid().ToString("N");

        GameObject instance = Instantiate(
            prefab,
            position,
            rotation
        );

        PersistentEntity entity =
            instance.GetComponent<PersistentEntity>();

        if (entity == null)
            entity = instance.AddComponent<PersistentEntity>();

        entity.Initialize(newId, prefab, true);

        EntityRecord record = CaptureEntity(entity);

        GetSceneRecords(sceneName)[newId] = record;

        Debug.Log("Spawned Entity: " + newId);

        return instance;
    }

    // =====================================
    // REMOVE ENTITY
    // =====================================

    public void RemoveEntity(PersistentEntity entity)
    {
        if (entity == null)
            return;

        string sceneName = entity.gameObject.scene.name;

        Dictionary<string, EntityRecord> records =
            GetSceneRecords(sceneName);

        if (entity.IsRuntimeSpawned)
        {
            // Entity được spawn động:
            // Xóa record để không tạo lại khi quay lại Scene.
            records.Remove(entity.EntityId);
        }
        else
        {
            // Entity đặt sẵn trong Scene:
            // Đánh dấu đã bị xóa để không xuất hiện lại.
            EntityRecord record = CaptureEntity(entity);
            record.isRemoved = true;
            record.isActive = false;

            records[entity.EntityId] = record;
        }

        Destroy(entity.gameObject);
    }

    // =====================================
    // RESTORE COMPONENT STATES
    // =====================================

    private void RestoreEntityState(
        PersistentEntity entity,
        EntityRecord record)
    {
        entity.transform.SetPositionAndRotation(
            record.position,
            record.rotation
        );

        foreach (EntityComponentSnapshot snapshot
                 in record.componentStates)
        {
            foreach (MonoBehaviour component
                     in entity.GetComponents<MonoBehaviour>())
            {
                if (component.GetType().AssemblyQualifiedName
                    != snapshot.componentType)
                    continue;

                if (component is IEntityState saveable)
                {
                    saveable.RestoreState(snapshot.jsonData);
                }
            }
        }

        entity.gameObject.SetActive(record.isActive);
    }

    // =====================================
    // RESTORE SCENE
    // =====================================

    private void OnSceneLoaded(
        Scene scene,
        LoadSceneMode mode)
    {
        RestoreScene(scene.name);
    }

    public void RestoreScene(string sceneName)
    {
        if (!sceneEntities.TryGetValue(
            sceneName,
            out Dictionary<string, EntityRecord> records))
            return;

        PersistentEntity[] existingEntities =
            FindObjectsByType<PersistentEntity>(
                FindObjectsInactive.Include,
                FindObjectsSortMode.None
            );

        HashSet<string> existingIds = new();

        // Khôi phục entity đã được đặt sẵn trong Scene
        foreach (PersistentEntity entity in existingEntities)
        {
            if (entity.gameObject.scene.name != sceneName)
                continue;

            string id = entity.EntityId;

            if (string.IsNullOrEmpty(id))
                continue;

            existingIds.Add(id);

            if (!records.TryGetValue(id, out EntityRecord record))
                continue;

            if (record.isRemoved)
            {
                entity.gameObject.SetActive(false);
                continue;
            }

            RestoreEntityState(entity, record);
        }

        // Tạo lại entity được spawn trong lúc chơi
        foreach (EntityRecord record in records.Values)
        {
            if (!record.isRuntimeSpawned ||
                record.isRemoved ||
                record.sourcePrefab == null)
                continue;

            if (existingIds.Contains(record.entityId))
                continue;

            GameObject instance = Instantiate(
                record.sourcePrefab,
                record.position,
                record.rotation
            );

            PersistentEntity entity =
                instance.GetComponent<PersistentEntity>();

            if (entity == null)
                entity = instance.AddComponent<PersistentEntity>();

            entity.Initialize(
                record.entityId,
                record.sourcePrefab,
                true
            );

            RestoreEntityState(entity, record);

            existingIds.Add(record.entityId);
        }

        Debug.Log("Restored Scene: " + sceneName);
    }
}
