using UnityEngine;
[CreateAssetMenu(menuName = "Inventory Data Item")]
public class InventoryItemData : ScriptableObject
{
    public string id; 
    public string displayName;
    public Sprite icone;
    public GameObject prefab;

    /*

    [CreateAssetMenu(menuName = "Inventory Data Item")] = allows you to create instances of InventoryItemData directly in the Unity editor
            Can Create a new inventory Item by: Assets -> Create -> Inventory Data Item

    ScriptableObject : data that doesn't need to be attached to a GameObject

    GameObject :  stores a reference to a prefab






    */

}
