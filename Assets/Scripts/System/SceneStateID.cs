
using UnityEngine;

public class SceneStateID : MonoBehaviour
{
    [SerializeField] private string uniqueId;

    public bool GetState(string stateName)
    {
        return SceneStateManager.Instance.GetBool(
            gameObject.scene.name,
            uniqueId,
            stateName
        );
    }

    public void SetState(string stateName, bool value)
    {
        SceneStateManager.Instance.SetBool(
            gameObject.scene.name,
            uniqueId,
            stateName,
            value
        );
    }
}
