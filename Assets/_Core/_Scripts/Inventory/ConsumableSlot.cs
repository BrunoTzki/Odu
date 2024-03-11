using UnityEngine;
using UnityEngine.UI;

public class ConsumableSlot : MonoBehaviour
{
    public Image icon;
    public Button removeButton;

    public Image itemImage;
    public Text itemDescription;

    Item item;
    public void AddItem(Item newItem)
    {
        item = newItem;

        icon.sprite = item.itemIconSprite;
        icon.enabled = true;
        removeButton.interactable = true;
    }   
    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        removeButton.interactable = false;
    }

    public void OnRemoveButton()
    {
        Inventory.instance.Remove(item);
        ClearSlot();
    }

    public void UseItem()
    {
        if (item != null)
        {
            item.Use();
            itemImage.sprite = item.itemSprite;
            itemDescription.text = item.itemDescription;
        }
    }

}
