
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    private string targetSpawnId;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Duplicate SceneTransitionManager!");
            Debug.LogWarning("Duplicate destroyed!");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadScene(string sceneName, string spawnId)
    {
        targetSpawnId = spawnId;

        // Lưu toàn bộ entity trong Scene hiện tại
        EntityManager.Instance?.SaveCurrentScene();

        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (string.IsNullOrEmpty(targetSpawnId))
            return;

        PlayerSpawnPoint[] spawnPoints =
            FindObjectsByType<PlayerSpawnPoint>(
                FindObjectsSortMode.None
            );

        foreach (PlayerSpawnPoint point in spawnPoints)
        {
            if (point.spawnId != targetSpawnId)
                continue;

            GameObject player =
                PersistentPlayer.Instance.gameObject;

            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.position = point.transform.position;
                rb.linearVelocity = Vector2.zero;
            }
            else
            {
                player.transform.position =
                    point.transform.position;
            }

            PlayerMovement movement = player.GetComponent<PlayerMovement>();

            if (movement != null)
                movement.RefreshInputActions();

            PlayerInteraction interaction = player.GetComponent<PlayerInteraction>();

            if (interaction != null)
                interaction.RefreshInputAction();

            Camera cameraFollow = FindFirstObjectByType<Camera>();

            if (cameraFollow != null)
            {
                cameraFollow.SetTarget(player.transform);
            }

            targetSpawnId = null;
            return;
        }

        Debug.LogError(
            "Cannot find Spawn Point: " + targetSpawnId
        );
    }
}
