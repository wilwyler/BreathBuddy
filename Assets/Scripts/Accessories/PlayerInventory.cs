using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Accessories {
    [CreateAssetMenu(menuName = "Item/Closet", fileName = "Closet Data")]
    public class PlayerInventory : ScriptableObject
    {
    public List<Item> InventoryItems;
    public int Coins;
    public int TotalBagSlots;
    
    }
}
