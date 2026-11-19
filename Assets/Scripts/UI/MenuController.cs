
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private string startScene;
    [SerializeField] private GameObject loadGamePanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject creditsPanel;

    public void NewGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(startScene);
    }
    public void OpenLoad()
    {
        settingsPanel.SetActive(true);
    }
    public void CloseLoad()
    {
        settingsPanel.SetActive(false);
    }
    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void OpenCredits()
    {
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
