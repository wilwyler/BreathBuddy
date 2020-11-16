using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Accessories
{
    public class StoreInteraction : MonoBehaviour
    {
        [SerializeField]
        private Inventory _inventoryList;
        [SerializeField]
        private GameObject _storeUI;

        public void Fill() {
            _storeUI.GetComponent<StoreUIController>().PopulateInventory(_inventoryList);
        }
}
}
