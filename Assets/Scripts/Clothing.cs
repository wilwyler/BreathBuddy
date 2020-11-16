using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Accessories
{
    [CreateAssetMenu(menuName = "Item/Clothing", fileName = "Clothing Name")]
    public class Clothing : Item
    {
        [Header("Clothing Properties"), Tooltip("Slot that clothing can be worn.")]
        public Types ItemType;
        public enum Types
        {
            Hat,
            Shoes
        }
    }
}