using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Accessories
{
    [CreateAssetMenu(menuName = "Item/Generic", fileName = "Generic File Name")]
    public class Item : ScriptableObject {
        [Header("General Properties")]
        public string Name = "New Item";
        public string Description = "New Description";
        public Sprite Sprite;
        [Header("Currency Properties")]
        public int PurchasePrice = 1;

    }
}