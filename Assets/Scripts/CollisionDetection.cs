using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour
{
    public Sprite redCircleSprite;
    public Sprite greenCircleSprite;

    private int _tooCloseCount = 0;
    private bool _tooClose = false;
    public int itemNearbyCount = 0;

    private struct AvailableItem
    {
        public int id;
        public GameObject item;
        public AvailableItem(int id, GameObject item)
        {
            this.id = id;
            this.item = item;
        }
    }
    private List<AvailableItem> _availableItemList = new List<AvailableItem>();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "CharacterRadius")
        {
            _tooCloseCount++;
            this.GetComponent<SpriteRenderer>().sprite = redCircleSprite;
            if (_tooClose == false)
            {
                _tooClose = true;
                UIManager.Instance.tooCloseCount++;
            }
        }

        if (collision.gameObject.tag == "Item")
        {
            itemNearbyCount++;

            int itemID = collision.transform.GetComponent<ItemController>().itemID;
            AvailableItem item = new AvailableItem(itemID, collision.gameObject);
            _availableItemList.Add(item);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "CharacterRadius")
        {
            _tooCloseCount--;
            // If no one is too close to this character
            if (_tooCloseCount == 0)
            {
                this.GetComponent<SpriteRenderer>().sprite = greenCircleSprite;
                UIManager.Instance.tooCloseCount--;
                _tooClose = false;
            }
        }

        if (collision.gameObject.tag == "Item")
        {
            itemNearbyCount--;

            int itemID = collision.transform.GetComponent<ItemController>().itemID;

            for (int i = 0; i < _availableItemList.Count; i++)
            {
                if (itemID == _availableItemList[i].id)
                {
                    _availableItemList.Remove(_availableItemList[i]);
                    break;
                }
            }
        }
    }

    // Determine if the items that the character can pick up is the target item (the item that the character needs to get). If yes, pick it up.
    public void PickUpItem(int targetItemID)
    {
        if (targetItemID != -1)
        {
            for (int i = 0; i < _availableItemList.Count; i++)
            {
                if (targetItemID == _availableItemList[i].id)
                {
                    // Debug.Log("targetItemID is " + targetItemID + " and _availableItemList[i].id is " + _availableItemList[i].id);
                    Destroy(_availableItemList[i].item);
                    this.transform.parent.GetComponent<CharacterContoller>().RetrieveItem(true);
                    break;
                }

                // All available items did not match target item
                if (i == _availableItemList.Count - 1)
                    this.transform.parent.GetComponent<CharacterContoller>().RetrieveItem(false);
            }
        }
    }
}
