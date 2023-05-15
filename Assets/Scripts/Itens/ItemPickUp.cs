using UnityEngine;

public class ItemPickUp : InteractableDummy
{
    public Item item;
    public override void Interact()
    {
        PickUp();
    }

    public void PickUp() 
    {
        Debug.Log("Item Pego");
        bool wasPickedUp = Inventory.instance.Add(item);
        if (wasPickedUp)
        {
            if (Inventory.instance != null) 
            {
                Destroy(gameObject);
                Debug.Log("Item Destroyed");
            }

        }
    }
}
