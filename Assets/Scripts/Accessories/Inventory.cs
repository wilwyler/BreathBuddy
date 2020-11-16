using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Accessories {
    [CreateAssetMenu(menuName = "Item/Inventory", fileName = "Inventory Data")]
    public class Inventory : ScriptableObject
    {
    public List<Item> InventoryItems = new List<Item>();
    }
}
