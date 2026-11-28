
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentSceneController : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string mainMenuSceneName = "MainMenu";

    [Header("Persistent Objects")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject gameplayCanvas;
    [SerializeField] private GameObject eventSys;


    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // Xử lý Scene hiện tại khi GameManager được khởi tạo
        ApplySceneSettings(SceneManager.GetActiveScene());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplySceneSettings(scene);
    }

    private void ApplySceneSettings(Scene scene)
    {
        bool isMainMenu = scene.name == mainMenuSceneName;

        // Ẩn khi vào Main Menu
        if (player != null)
            player.SetActive(!isMainMenu);

        if (gameplayCanvas != null)
            gameplayCanvas.SetActive(!isMainMenu);

        if (eventSys != null)
            eventSys.SetActive(!isMainMenu);

        Debug.Log(
            "Scene: " + scene.name +
            " | Main Menu: " + isMainMenu
        );
    }
}
