using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Accessories {

    public class PlayerUIController : MonoBehaviour
    {
        [SerializeField]
        public PlayerInventory _playerInventory;
        private GameObject _playerInventoryWindow;
        private Text _coinText;
        [SerializeField]
        private GameObject _itemTemplate;
        private Transform _scrollViewContent;
        private void Awake()
        {
            //_playerInventoryWindow = transform.parent.parent.FindChild("Closet").gameObject;
            //_BagSpaceText = _playerInventoryWindow.transform.FindChild("Footer/BagDetails/Amount").GetComponent<Text>();
            //_coinText = transform.FindChild("Coin Count/Text");
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "Closet") {
                _scrollViewContent = transform.Find("Scroll View/Viewport/Content");
                _coinText = _scrollViewContent.transform.parent.parent.parent.Find("Coin Count/Text").GetComponent<Text>();
            }
            else {
                _playerInventoryWindow = transform.parent.Find("Closet").gameObject;
                _scrollViewContent = transform.Find("Scroll View/Viewport/Content");
                _coinText = _scrollViewContent.transform.parent.parent.parent.Find("Coin Count/Text").GetComponent<Text>();
            }
        }

        private void Start()
        {
            //update bag text
            //_playerInventory.TotalBagSlots = _playerInventory.InventoryItems.Count;

            //Create bag slots
            for (int i = 0; i < _playerInventory.InventoryItems.Count; i++)
            {
                GameObject newItem = Instantiate(_itemTemplate, _scrollViewContent);
                //newItem.transform.localScale = Vector3.one;
            }

            //Add Inventory Slots
            for (int i = 0; i < _playerInventory.InventoryItems.Count; i++)
            {
                _scrollViewContent.GetChild(i).GetComponent<Image>().sprite = _playerInventory.InventoryItems[i].Sprite;
            }

            UpdateCurrency();
        }

        public void UpdateCurrency() {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "Closet") {
                _coinText.text = _playerInventory.Coins.ToString();
            }
            else {
                _coinText.text = _playerInventory.Coins.ToString();
            }
        }

        public void PurchaseItem(Item purchasedItem){
            _playerInventory.Coins -= purchasedItem.PurchasePrice;
            UpdateCurrency();
            _playerInventory.InventoryItems.Add(purchasedItem);
            GameObject newItem = Instantiate(_itemTemplate, _scrollViewContent);
            _scrollViewContent.GetChild(_playerInventory.InventoryItems.Count - 1).GetComponent<Image>().sprite = purchasedItem.Sprite;
        }


    }
}

