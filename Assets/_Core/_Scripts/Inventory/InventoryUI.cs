using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Transform consumablesParent;

    Inventory inventory;

    ConsumableSlot[] consumablesSlots;

    // Start is called before the first frame update
    void Start()
    {
        inventory = Inventory.instance; 
        inventory.onItemChangedCallback += UpdateUI;

        consumablesSlots = consumablesParent.GetComponentsInChildren<ConsumableSlot>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateUI()
    {
        for (int i = 0; i < consumablesSlots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                consumablesSlots[i].AddItem(inventory.items[i]);
            }
            else
            {
                consumablesSlots[i].ClearSlot();
            }
        }
    }
}
