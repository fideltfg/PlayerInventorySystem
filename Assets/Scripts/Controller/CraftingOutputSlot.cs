using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace PlayerInventorySystem
{

    /// <summary>
    /// Controller for the crafting panel output slot.
    /// 
    /// This class provide methods to generate new items based on the contents of the crafting slots.
    /// </summary>
    public class CraftingOutputSlot : SlotController, IPointerDownHandler
    {
        public override void Update ()
        {
            base.Update();
            Item craftedItem = GetItemByIngredients(GivenitemsAsIngredients());
            Slot.SetItem(craftedItem);
        }

        /// <summary>
        /// method to return an item based on the items provided by the player.
        /// </summary>
        /// <param name="ingredients"></param>
        /// <returns></returns>
        private Item GetItemByIngredients (string ingredients)
        {
            if (ingredients.Length <= 0)
            {
                return null;
            }

            foreach (ItemData itemData in InventoryController.Instance.ItemCatalog.list)
            {
                string i = itemData.recipe.Ingredients;
                if (i.Equals(ingredients))
                {
                    return Item.New(itemData, itemData.craftCount);
                }
            }
            return null;
        }


        /// <summary>
        /// Method to return the crafting code( aka ingredients ) for the items provided in the crafting slots
        /// </summary>
        /// <returns>returns the sting crafting code generated from the items provided</returns>
        private string GivenitemsAsIngredients ()
        {
            string r = "";
            foreach (SlotController sc in InventoryController.Instance.CraftingPanel.SlotList)
            {
                if (sc.Slot.Item == null)
                {
                    r += "X";
                }
                else
                {
                    string x = sc.Slot.Item.data.id.ToString();
                    if ((x.Length & 1) != 0)
                    {
                        string c = x.PadLeft(2, '0');
                        r += c;
                    }
                    else
                    {
                        r += x;
                    }
                }
            }
            char[] t = { 'X' };
            return r.Trim(t);
        }

        /// <summary>
        /// consumes the items needed to craft the item in the output slot when the user picks it up.
        /// </summary>
        /// <param name="count">The number of consumptions</param>
        private void Consume ()
        {
            foreach (SlotController sc in InventoryController.Instance.CraftingPanel.SlotList)
            {
                if (sc.Slot.Item != null)
                {
                    sc.Slot.IncermentStackCount(-1);
                    if (sc.Slot.ItemStackCount <= 0)
                    {
                        sc.Slot.SetItem(null);
                    }

                }
            }
        }

        public override void OnPointerDown (PointerEventData eventData)
        {
            // if there is an item in the output slot
            if (!Slot.IsEmpty && Slot.Item != null)
            {
                if (HeldItem == null)
                {
                    // if the player is holding down shift
                    if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    {
                        // calculate how many items can be crafted with the items in the crafting array
                        int craftCount = MaxCraftCount();

                        // calculate how many items can be fit in to the inventory
                        int ItemBarSpaces = InventoryController.ItemBarInventory.EmptySlotCount * Slot.Item.data.maxStackSize;
                        int inventorySpaces = InventoryController.PlayerInventory.EmptySlotCount * Slot.Item.data.maxStackSize;

                        // limit the craft amount so they fit in the inventory spaces availiable
                        craftCount = Mathf.Clamp(craftCount, 0, inventorySpaces + ItemBarSpaces);


                        if (craftCount <= ItemBarSpaces)
                        {
                            for (int i = 0; i < craftCount; i += Slot.Item.data.craftCount)
                            {
                                if (InventoryController.ItemBarInventory.AddItem(Item.New(Slot.Item.data, Slot.Item.data.craftCount)))
                                {
                                    Consume();
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < ItemBarSpaces; i += Slot.Item.data.craftCount)
                            {
                                if (InventoryController.ItemBarInventory.AddItem(Item.New(Slot.Item.data, Slot.Item.data.craftCount)))
                                {
                                    Consume();
                                }
                            }
                            for (int i = 0; i < (craftCount - ItemBarSpaces); i += Slot.Item.data.craftCount)
                            {
                                if (InventoryController.PlayerInventory.AddItem(Item.New(Slot.Item.data, Slot.Item.data.craftCount)))
                                {
                                    Consume();
                                }
                            }
                        }
                    }
                    else
                    {
                        // pick up one crafted stack

                        // clone this item  to the held Item 
                        HeldItem = Slot.Item.Clone();
                        Slot.SetItem(null);
                        Consume();
                    }
                }

                // if held item is not null but held item is the same as this Item
                else
                {

                    if (HeldItem.data.id == Slot.Item.data.id)
                    {
                        // and held stack is not full
                        if (HeldItem.StackCount <= HeldItem.data.maxStackSize - Slot.Item.data.craftCount)
                        {
                            // pick up more
                            Consume();
                            // heldItem.stackCount += Item.data.craftCount;
                            HeldItem.AddToStack(Slot.Item.data.craftCount);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// method to return the max number of items that can be crafted with the contents of the crafting array
        /// </summary>
        private int MaxCraftCount ()
        {
            int c = 123456;
            foreach (SlotController slotController in InventoryController.Instance.CraftingPanel.SlotList)
            {
                if (slotController.Slot.Item != null)
                {
                    if (slotController.Slot.Item.StackCount < c)
                    {
                        c = slotController.Slot.Item.StackCount;
                    }
                }
            }

            if (c == 123456)
            {
                return 0;
            }
            return c * this.Slot.Item.data.craftCount;
        }

        /// <summary>
        /// Method to swap the item in the crafting slot with a slot on the itemBar
        /// </summary>
        /// <param name="ItemBarSlotID">The ID of the slot on the ItemBar t which this item is to be sent/swapped</param>
        public override void SwapWithItemBarSlot (int ItemBarSlotID)
        {

            // if the target slot is empty
            if (InventoryController.Instance.ItemBar.SlotList[ItemBarSlotID].Slot.Item == null)
            {
                InventoryController.Instance.ItemBar.SlotList[ItemBarSlotID].Slot.SetItem(Slot.Item);
                InventoryController.Instance.ItemBar.SlotList[ItemBarSlotID].UpdateSlotUI();
                Consume();
            }
            // if the target slot has an item the same as this
            else if (InventoryController.Instance.ItemBar.SlotList[ItemBarSlotID].Slot.Item.data.id == Slot.Item.data.id)
            {
                InventoryController.Instance.ItemBar.SlotList[ItemBarSlotID].Slot.Item.AddToStack(Slot.Item.data.craftCount);
                InventoryController.Instance.ItemBar.SlotList[ItemBarSlotID].UpdateSlotUI();
                Consume();
            }
           
        }

    }
}