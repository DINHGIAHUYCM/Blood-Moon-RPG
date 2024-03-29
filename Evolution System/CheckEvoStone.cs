using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CheckEvoStone : MonoBehaviour
{
    public TextMeshProUGUI itemStatusText;
    public string itemName;
    public GameObject[] activeObjects;
    public GameObject[] inactiveObjects;

    void Update()
    {
        UpdateItemStatus();
        UpdateObjectStatus();
    }

    void UpdateItemStatus()
    {
        int itemCount = GetItemCount();
        if (itemCount >= 2)
        {
            itemStatusText.text = "1/1";
        }
        else
        {
            itemStatusText.text = "0/1!";
        }
    }

    void UpdateObjectStatus()
    {
        int itemCount = GetItemCount();

        if (itemCount > 1)
        {
            SetObjectStatus(activeObjects, true);
            SetObjectStatus(inactiveObjects, false);
        }
        else
        {
            SetObjectStatus(activeObjects, false);
            SetObjectStatus(inactiveObjects, true);
        }
    }

    int GetItemCount()
    {
        string inventory = PlayerPrefs.GetString("Inventory", "");
        List<string> inventoryItems = new List<string>(inventory.Split(','));

        List<string> matchingItems = inventoryItems.FindAll(item => item == itemName);
        return matchingItems.Count;
    }

    void SetObjectStatus(GameObject[] objects, bool isActive)
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(isActive);
        }
    }
}