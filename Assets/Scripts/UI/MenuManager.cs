using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [Header("Menus to Close")]
    [SerializeField] private GameObject[] menus;

    [Header("Settings")]
    [SerializeField] private bool resumeGameOnClose = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            CloseAllMenus();
        }
    }

    public void CloseAllMenus()
    {
        bool closedAnyMenu = false;

        foreach (GameObject menu in menus)
        {
            if (menu != null && menu.activeSelf)
            {
                menu.SetActive(false);
                closedAnyMenu = true;
            }
        }

        // Chỉ tiếp tục game nếu thực sự có menu được đóng
        if (closedAnyMenu && resumeGameOnClose)
        {
            Time.timeScale = 1f;
        }
    }

    public void RegisterMenu(GameObject menu)
    {
        if (menu == null)
            return;

        foreach (GameObject existingMenu in menus)
        {
            if (existingMenu == menu)
                return;
        }

        System.Array.Resize(ref menus, menus.Length + 1);
        menus[menus.Length - 1] = menu;
    }
}