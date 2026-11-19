
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewCharacter",
    menuName = "Delta/Character Definition"
)]
public class CharacterDefinition : ScriptableObject
{
    [Header("Identity")]
    public string characterId;
    public string characterName;

    [TextArea]
    public string description;

    [Header("Visual")]
    public GameObject visualPrefab;
    public Sprite portrait;

    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;

    [Header("Combat")]
    public int invSlot = 12;
    public int maxHealth = 5;
    public int meleeDmg = 3;
    public int rangedDmg = 5;
    public int maxCharge = 3;
}
