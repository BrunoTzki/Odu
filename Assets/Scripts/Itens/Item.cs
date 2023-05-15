using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    //create item variables
    public string itemName;
    public Sprite itemSprite;
    public Sprite itemIconSprite;
    public string itemDescription;
    public int itemID;
    public int itemAmount;

    public bool isStackable;
    public bool isEquippable;
    public bool isConsumable;
    public bool isKeyItem;
    public bool isFerramenta;
    public bool isGuia;
    public bool isQuestItem;
    public bool isMaterial;
    public bool isCraftable;
    public bool isIngredient;

    public virtual void Use()
    {
        //use item
        //something might happen
        //
        Debug.Log("Using " + itemName);
    }
}
