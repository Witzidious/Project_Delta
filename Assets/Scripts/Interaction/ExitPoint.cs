
using UnityEngine;

public class SceneExit : InteractableBase
{
    [Header("Scene Transition")]
    [SerializeField] private string targetScene;
    [SerializeField] private string targetSpawnId;

    public override void Interact()
    {
        Debug.Log("Scene Exit Interacted!");
        if (SceneTransitionManager.Instance == null)
        {
            Debug.LogError("SceneTransitionManager is missing!");
            return;
        }

        SceneTransitionManager.Instance.LoadScene(
            targetScene,
            targetSpawnId
        );
    }
}
