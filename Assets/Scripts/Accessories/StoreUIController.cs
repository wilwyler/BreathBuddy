using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEditor;
//using UnityEngine.UI.dll;

namespace Assets.Scripts.Accessories {
public class StoreUIController : MonoBehaviour
{
    public Inventory StoreInventory;
    [SerializeField]
    private GameObject ItemTemplate;
    //private GameObject CurrencyTemplate;
     private Transform _scrollViewContent;
    
     private PlayerUIController _playerController;

    private void Start()
        {
            _playerController = FindObjectOfType<PlayerUIController>();
        }
    public void PopulateInventory(Inventory inventoryList) {

        ClearInventory();
        StoreInventory = inventoryList;
        Transform ScrollViewContent = transform.Find("Scroll View/Viewport/Content");

        foreach (var item in inventoryList.InventoryItems) {
            GameObject newItem = Instantiate(ItemTemplate, ScrollViewContent);
            newItem.transform.Find("Image").GetComponent<Image>().sprite = item.Sprite;
           newItem.transform.Find("Name").GetComponent<Text>().text = item.Name;
           newItem.transform.Find("Description").GetComponent<Text>().text = item.Description;
           newItem.transform.Find("Price").GetComponent<Text>().text = item.PurchasePrice.ToString();

           newItem.transform.Find("Buy").GetComponent<Button>().onClick.AddListener(BuyOnClick);
        }
        
    }
    /// <summary>
        /// Clears out any existing inventory UI items
        /// </summary>
        public void ClearInventory()
        {
            StoreInventory = null;
            //Since this starts out as disabled, we need to do a check the first time we try to access the content element, as we may not have a reference to it.
            if (_scrollViewContent == null)
            {
                _scrollViewContent = transform.Find("Scroll View/Viewport/Content");
            }

            foreach (Transform child in _scrollViewContent)
            {
                Destroy(child.gameObject);
            }
        }
        public void BuyOnClick() {
            Item purchasedItem = StoreInventory.InventoryItems.Find(x => x.Name.Equals(EventSystem.current.currentSelectedGameObject.transform.parent.Find("Name").GetComponent<Text>().text));
            //Item purchasedItem;
            if (purchasedItem == null) {
                Debug.Log("Unable to find item in store.");
                return;
            }
            else if (purchasedItem.PurchasePrice >= _playerController._playerInventory.Coins) {
                Debug.Log("Cannot afford item");
                return;
            }
            _playerController.PurchaseItem(purchasedItem);
            StoreInventory.InventoryItems.Remove(purchasedItem);
            PopulateInventory(StoreInventory);
        }
}
}

