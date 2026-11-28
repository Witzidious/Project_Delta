
using Unity.VisualScripting;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    public static DontDestroyOnLoad Instance { get; private set; }
    private void Awake()
    {

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
