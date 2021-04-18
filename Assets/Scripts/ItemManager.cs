using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private static ItemManager _instance;
    public static ItemManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("The ItemManager is null");
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public string[] itemLibrary;
    public Sprite[] itemSprites;
    public GameObject itemPrefab;
    private List<int> itemAssigned = new List<int>();

    // Called by GameManager to assign item randomly
    public int AssignItem()
    {
        int id = Random.Range(0, itemLibrary.Length);
        string item = itemLibrary[id];
        itemAssigned.Add(id);
        return id;
    }

    // Called by GameManager to spawn items
    public void SpawnItem()
    {
        for (int i = 0; i < itemAssigned.Count; i++)
        {
            float x = Random.Range(-7.5f, 7.5f);
            float y = Random.Range(-2.0f, 1.5f);
            GameObject newItem = Instantiate(itemPrefab, new Vector3(x, y, 0), Quaternion.identity);
            newItem.GetComponent<SpriteRenderer>().sprite = itemSprites[itemAssigned[i]];
            newItem.GetComponent<ItemController>().itemID = itemAssigned[i];
        }
    }

    public void ResetItem()
    {
        itemAssigned.Clear();
    }
}
