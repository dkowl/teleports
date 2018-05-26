using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAddMessage : IMessage
{
    private ItemData item;
    private bool firstItemOfThatTypeInInventory;

    public ItemAddMessage(ItemData item, bool firstItemOfThatTypeInInventory)
    {
        this.item = item;
        this.firstItemOfThatTypeInInventory = firstItemOfThatTypeInInventory;
    }

    public ItemData Item => item;
    public bool FirstItemOfThatTypeInInventory => firstItemOfThatTypeInInventory;
}
