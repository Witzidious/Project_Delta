
using UnityEngine;

public class CharacterSetup : MonoBehaviour
{
    [SerializeField]
    private CharacterDefinition characterData;

    [SerializeField]
    private Transform visualParent;

    private GameObject currentVisual;

    public CharacterDefinition CharacterData => characterData;

    public void SetCharacter(CharacterDefinition newCharacter)
    {
        if (newCharacter == null)
        {
            Debug.LogError("Character Definition is missing!");
            return;
        }

        if (newCharacter.visualPrefab == null)
        {
            Debug.LogError("Visual Prefab is missing!");
            return;
        }

        characterData = newCharacter;

        // Xóa tất cả Visual cũ bên trong visualParent
        foreach (Transform child in visualParent)
        {
            Destroy(child.gameObject);
        }

        // Tạo Visual mới
        currentVisual = Instantiate(
            characterData.visualPrefab,
            visualParent
        );

        currentVisual.transform.localPosition = Vector3.zero;
        currentVisual.transform.localRotation = Quaternion.identity;
        currentVisual.transform.localScale = Vector3.one;

        // Tìm Animator trong Visual vừa tạo
        Animator visualAnimator = currentVisual.GetComponentInChildren<Animator>(true);

        if (visualAnimator == null)
        {
            Debug.LogError(
                "PlayerCharacterSetup: No Animator found in Visual Prefab!"
            );
            return;
        }

        // Truyền Animator cho PlayerMovement
        PlayerMovement movement = GetComponent<PlayerMovement>();

        if (movement != null)
        {
            movement.SetAnimator(visualAnimator);

            movement.SetMovementSpeeds(
                characterData.walkSpeed,
                characterData.sprintSpeed
            );
        }
        else
        {
            Debug.LogError("PlayerMovement component not found!");
        }

        Debug.Log(
            "Character loaded: " + characterData.characterName
        );
    }

    private void Start()
    {
        if (characterData != null)
        {
            SetCharacter(characterData);
        }
    }
}
