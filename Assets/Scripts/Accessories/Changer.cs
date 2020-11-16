/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static JellySprite;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
namespace Assets.Scripts.Accessories {
public class Changer : MonoBehaviour
{
    [SerializeField]
    public PlayerInventory _playerInventory;
    [SerializeField]
    public JellySprite blob;
    // Start is called before the first frame update

    public void Change() {
        Item newItem = _playerInventory.InventoryItems.Find(x => x.Name.Equals(EventSystem.current.currentSelectedGameObject.transform.parent.FindChild("Item Type").GetComponent<Text>().text));
        blob.FindChild(newItem.ItemType).GetComponent<Sprite>() = newItem.Sprite;
    }    
}  
}*/
