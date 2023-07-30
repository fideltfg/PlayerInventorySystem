﻿namespace PlayerInventorySystem.Serial
{
    using System;
    using UnityEngine;

    [Serializable]
    internal class SerialSaveDataObject
    {
        [SerializeField] internal int PlayerInventoryCapacity;
        [SerializeField] private SerialInventory[] inventories;
        [SerializeField] private SerialChest[] chests;
        [SerializeField] private SerialRect[] panelLocations;
        [SerializeField] private SerialDroppedItem[] droppedItems;
        [SerializeField] private SerialPlacedItem[] placedItems;



        /// <summary>
        /// An array of all the inventories in the system serialized.
        /// </summary>
        public SerialInventory[] Inventories
        {
            get { return inventories; }
            set { inventories = value; }
        }

        /// <summary>
        /// An array of all the chests in the system serialized.
        /// </summary>
        public SerialChest[] Chests
        {
            get { return chests; }
            set { chests = value; }
        }

        /// <summary>
        /// An array of all the panel locations in the system serialized.
        /// </summary>
        public SerialRect[] PanelLocations
        {
            get { return panelLocations; }
            set { panelLocations = value; }
        }

        /// <summary>
        /// An array of all the dropped or spawned items in the game world at save time serialized.
        /// </summary>
        public SerialDroppedItem[] DroppedItems
        {
            get { return droppedItems; }
            set { droppedItems = value; }
        }

        public SerialPlacedItem[] PlacedItems
        {
            get { return placedItems; }
            set { placedItems = value; }
        }

        /// <summary>
        /// Constructor to create a new SerialSaveDataObject with the provided data.
        /// </summary>
        /// <param name="inventories">An array of SerialInventory objects representing the inventories in the system.</param>
        /// <param name="chests">An array of SerialChest objects representing the chests in the system.</param>
        /// <param name="panelLocations">An array of SerialRect objects representing the panel locations in the system.</param>
        /// <param name="droppedItems">An array of SerialDroppedItem objects representing the dropped or spawned items in the game world.</param>
        public SerialSaveDataObject(SerialInventory[] inventories, SerialChest[] chests, SerialRect[] panelLocations, SerialDroppedItem[] droppedItems, SerialPlacedItem[] placedItems)
        {
            Inventories = inventories;
            Chests = chests;
            PanelLocations = panelLocations;
            DroppedItems = droppedItems;
            PlacedItems = placedItems;
        }
    }

}