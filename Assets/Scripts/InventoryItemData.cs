using UnityEngine;
[CreateAssetMenu(menuName = "Inventory Data Item")]
public class InventoryItemData : ScriptableObject
{
    public string id; 
    public string displayName;
    public Sprite icone;
    public GameObject prefab;

}
