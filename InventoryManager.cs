using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [SerializeField]
    Button[] inventoryButtons;

    int slot1;

    Color savedColor;

    [SerializeField]
    Inventory playerInventory, treasureInventory;

    [SerializeField]
    ItemList masterItemList;

    // Start is called before the first frame update
    void Start()
    {
        slot1 = -1;
        playerInventory = new Inventory(9);
        treasureInventory = new Inventory(9);

        for (int i = 0; i < playerInventory.slots.Length; i++)
        {
            playerInventory.slots[i] = Random.Range(0, masterItemList.items.Length);
            //if first item slot in the player inventory is not a weapon then ser it to empty
            if (i == 0 && masterItemList.items[playerInventory.slots[i]].itemType != ItemTypes.Weapon)
            {
                playerInventory.slots[i] = 0;
            }
        }
        for (int i = 0; i < treasureInventory.slots.Length; i++)
        {
            treasureInventory.slots[i] = Random.Range(0, masterItemList.items.Length);
        }
        UpdateButtons();
    }

    public void UpdateButtons()
    {
        for (int i = 0; i < playerInventory.slots.Length; i++)
        {
            inventoryButtons[i].GetComponentInChildren<Text>().text = masterItemList.items[playerInventory.slots[i]].itemName;
        }
        for (int i = 0; i < treasureInventory.slots.Length; i++)
        {
            inventoryButtons[i + 9].GetComponentInChildren<Text>().text = masterItemList.items[treasureInventory.slots[i]].itemName;
        }
    }

    public void SwapItem(int index)
    {
        int temp;
        ColorBlock colors;

        if (slot1 == -1)
        {
            slot1 = index;
            colors = inventoryButtons[slot1].colors;
            savedColor = colors.selectedColor;
            colors.selectedColor = Color.blue;
            inventoryButtons[slot1].colors = colors;
        }
        else
        {
            colors = inventoryButtons[slot1].colors;
            colors.selectedColor = savedColor;
            inventoryButtons[slot1].colors = colors;

            bool canSwap = false;

            if (slot1 < playerInventory.slots.Length)
            {
                temp = playerInventory.slots[slot1];
                if (index < playerInventory.slots.Length)
                {
                    if (slot1 == 0 && (masterItemList.items[playerInventory.slots[index]].itemType == ItemTypes.Weapon ||
                        masterItemList.items[playerInventory.slots[index]].itemType == ItemTypes.Empty))
                    {
                        canSwap = true;
                    }

                    if (slot1 != 0 && index != 0)
                    {
                        canSwap = true;
                    }

                    if (index == 0 && (masterItemList.items[playerInventory.slots[slot1]].itemType == ItemTypes.Weapon ||
                        masterItemList.items[playerInventory.slots[slot1]].itemType == ItemTypes.Empty))
                    {
                        canSwap = true;
                    }

                    if (canSwap)
                    {
                        playerInventory.slots[slot1] = playerInventory.slots[index];
                        playerInventory.slots[index] = temp;
                    }
                }
                else
                {
                    //If playerInventory is hosen first and treasureInventory is chosen second
                    if (slot1 == 0 && (masterItemList.items[treasureInventory.slots[index - 9]].itemType == ItemTypes.Weapon ||
                        masterItemList.items[treasureInventory.slots[index - 9]].itemType == ItemTypes.Empty))
                    {
                        canSwap = true;
                    }
                    if (slot1 != 0 && index != 0)
                    {
                        canSwap = true;
                    }

                    if (canSwap)
                    {
                        playerInventory.slots[slot1] = treasureInventory.slots[index - 9];
                        treasureInventory.slots[index - 9] = temp;
                    }


                }
            }
            else
            {
                temp = treasureInventory.slots[slot1 - 9];
                if (index < playerInventory.slots.Length)
                {
                    if(index == 0 && (masterItemList.items[treasureInventory.slots[slot1 - 9]].itemType == ItemTypes.Weapon ||
                        masterItemList.items[treasureInventory.slots[slot1 - 9]].itemType == ItemTypes.Empty))
                    {
                        canSwap = true;
                    }

                    if(slot1 != 0 && index != 0)
                    {
                        canSwap = true;
                    }

                    if (canSwap)
                    {
                        treasureInventory.slots[slot1 - 9] = playerInventory.slots[index];
                        playerInventory.slots[index] = temp;
                    }

                }
                else
                {
                    treasureInventory.slots[slot1 - 9] = treasureInventory.slots[index - 9];
                    treasureInventory.slots[index - 9] = temp;
                }
            }
            slot1 = -1;
            UpdateButtons();
        }


    }


}

[System.Serializable]
public class Inventory
{
    int maxSlots;
    public int[] slots;

    public Inventory()
    {
        maxSlots = 9;
        slots = new int[maxSlots];
    }

    public Inventory(int maxSlots)
    {
        slots = new int[maxSlots];
    }

}

[System.Serializable]
public class Item
{
    public string itemName;
    public ItemTypes itemType;

    public Item()
    {
        itemName = "Empty";
        itemType = ItemTypes.Empty;
    }

    public Item(string name, ItemTypes itemType)
    {
        itemName = name;
        this.itemType = itemType;
    }
}

public enum ItemTypes
{
    Empty = 0,
    Weapon = 1,
    Armor = 2,
    Consumable = 3,
    Misc = 4
}

[System.Serializable]
public class ItemList
{
    public Item[] items;
}