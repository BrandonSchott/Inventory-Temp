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

    // Start is called before the first frame update
    void Start()
    {
        slot1 = -1;
        playerInventory = new Inventory(9);
        treasureInventory = new Inventory(9);

        for (int i = 0; i < playerInventory.slots.Length; i++)
        {
            playerInventory.slots[i] = Random.Range(0, 9);
        }
        for (int i = 0; i < treasureInventory.slots.Length; i++)
        {
            treasureInventory.slots[i] = Random.Range(0, 9);
        }
        UpdateButtons();
    }

    public void UpdateButtons()
    {
        for (int i = 0; i < playerInventory.slots.Length; i++)
        {
            inventoryButtons[i].GetComponentInChildren<Text>().text = playerInventory.slots[i].ToString();
        }
        for (int i = 0; i < treasureInventory.slots.Length; i++)
        {
            inventoryButtons[i + 9].GetComponentInChildren<Text>().text = treasureInventory.slots[i].ToString();
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

            if (slot1 < playerInventory.slots.Length)
            {
                temp = playerInventory.slots[slot1];
                if (index < playerInventory.slots.Length)
                {
                    playerInventory.slots[slot1] = playerInventory.slots[index];
                    playerInventory.slots[index] = temp;
                }
                else
                {
                    playerInventory.slots[slot1] = treasureInventory.slots[index - 9];
                    treasureInventory.slots[index - 9] = temp;
                }
            }
            else
            {
                temp = treasureInventory.slots[slot1 - 9];
                if(index < playerInventory.slots.Length)
                {
                    treasureInventory.slots[slot1 - 9] = playerInventory.slots[index];
                    playerInventory.slots[index] = temp;
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