
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private string startScene;
    [SerializeField] private GameObject loadGamePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    [Header("Scene Transition")]
    [SerializeField] private string targetSpawnId;

    public void NewGame()
    {
        Time.timeScale = 1f;
        SceneTransitionManager.Instance.LoadScene(
            startScene,
            targetSpawnId
        );
    }
    public void OpenLoad()
    {
        settingsPanel.SetActive(false);
        loadGamePanel.SetActive(true);
        creditsPanel.SetActive(false);
    }
    public void CloseLoad()
    {
        loadGamePanel.SetActive(false);
    }
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
        loadGamePanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void OpenCredits()
    {
        settingsPanel.SetActive(false);
        loadGamePanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
