using UnityEngine;

[CreateAssetMenu(
    fileName = "ScriptableObject",
    menuName = "Delta/Inventory Item"
)]
public class InventoryItem : ScriptableObject
{
    [Header("World Item")]
    public GameObject worldPrefab;
    public string itemId;
    public string itemName;
    public string itemStatus;

    [TextArea]
    public string description;

    public Sprite icon;
}
