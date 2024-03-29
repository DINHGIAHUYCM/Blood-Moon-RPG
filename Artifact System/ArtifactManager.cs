using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

[System.Serializable]
public class Artifact
{
    public string name;
    public ItemType type;
    public Star star;
    public Season season;
    public Sprite image;
}

public enum ItemType
{
    Helmet,
    Shoe,
    Coach,
    Gloves
}

public enum Star
{
    S5,
    S4,
    S3,
    S2,
    S1
}

public enum Season
{
    SS1,
    SS2
}

public class ArtifactManager : MonoBehaviour
{
    public Artifact[] artifacts;
    public Image helmetImage;
    public Image shoeImage;
    public Image coachImage;
    public Image glovesImage;

    private void Start()
    {
        LoadImagesFromPlayerPrefs();
    }

    public void LoadImagesFromPlayerPrefs()
    {
        foreach (Artifact artifact in artifacts)
        {
            Image image = GetImageByType(artifact.type);
            if (image != null)
            {
                LoadImageFromPlayerPrefs(artifact, image);
            }
        }
    }

    private Image GetImageByType(ItemType type)
    {
        switch (type)
        {
            case ItemType.Helmet:
                return helmetImage;
            case ItemType.Shoe:
                return shoeImage;
            case ItemType.Coach:
                return coachImage;
            case ItemType.Gloves:
                return glovesImage;
            default:
                return null;
        }
    }

    private void LoadImageFromPlayerPrefs(Artifact artifact, Image image)
    {
        string spritePath = "Sprites/" + artifact.type.ToString() + "/" + artifact.star.ToString() + "_" + artifact.season.ToString();
        Sprite sprite = Resources.Load<Sprite>(spritePath);
        if (sprite != null)
        {
            artifact.image = sprite;
            image.sprite = sprite;
        }
        else
        {
            Debug.LogError("Sprite not found at path: " + spritePath);
        }
    }

    public void RandomizeItems()
    {
        RandomizeItemOfType(ItemType.Helmet);
        RandomizeItemOfType(ItemType.Shoe);
        RandomizeItemOfType(ItemType.Coach);
        RandomizeItemOfType(ItemType.Gloves);
    }

    public void RandomizeItemOfType(ItemType type)
    {
        Artifact[] itemsOfType = GetItemsOfType(type);
        if (itemsOfType.Length == 0)
        {
            Debug.Log("No items of type " + type + " found.");
            return;
        }

        int randomStar = GetRandomStar();
        Artifact randomItem = GetRandomItem(itemsOfType, randomStar);
        PlayerPrefs.SetInt(type.ToString(), (int)randomItem.star);
        PlayerPrefs.SetString(type.ToString() + "_Season", randomItem.season.ToString());

        Debug.Log("Randomized item of type " + type + ": " + randomItem.name);
        Debug.Log("Stored star value for " + type + ": " + randomItem.star);
        Debug.Log("Stored season value for " + type + ": " + randomItem.season);
    }

    public void ChangeStar(string type)
    {
        if (!Enum.TryParse(type, out ItemType itemType))
        {
            Debug.Log("Invalid item type: " + type);
            return;
        }

        Artifact[] itemsOfType = GetItemsOfType(itemType);
        if (itemsOfType.Length == 0)
        {
            Debug.Log("No items of type " + itemType + " found.");
            return;
        }

        int currentStar = PlayerPrefs.GetInt(type, (int)Star.S1);
        Star newStar;
        int randomValue = UnityEngine.Random.Range(0, 101);

        if (randomValue <= 25 && currentStar < (int)Star.S5)
        {
            newStar = (Star)(currentStar + 1);
        }
        else
        {
            newStar = Star.S1;
        }

        Artifact randomItem = GetRandomItem(itemsOfType, (int)newStar);
        PlayerPrefs.SetInt(type, (int)randomItem.star);
        PlayerPrefs.SetString(type + "_Season", randomItem.season.ToString());

        Debug.Log("Changed star of item of type " + itemType + " to " + randomItem.star);
        Debug.Log("Stored star value for " + itemType + ": " + randomItem.star);
        Debug.Log("Stored season value for " + itemType + ": " + randomItem.season);
    }

    private int GetRandomStar()
    {
        int randomValue = UnityEngine.Random.Range(1, 101);
        if (randomValue <= 1)
        {
            return (int)Star.S5;
        }
        else if (randomValue <= 6)
        {
            return (int)Star.S4;
        }
        else if (randomValue <= 26)
        {
            return (int)Star.S3;
        }
        else if (randomValue <= 56)
        {
            return (int)Star.S2;
        }
        else
        {
            return (int)Star.S1;
        }
    }

    private Artifact GetRandomItem(Artifact[] items, int star)
    {
        Artifact[] itemsWithStar = items.Where(item => (int)item.star == star).ToArray();
        if (itemsWithStar.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, itemsWithStar.Length);
            return itemsWithStar[randomIndex];
        }
        else
        {
            Debug.Log("No items found with star value " + star);
            return null;
        }
    }

    private Artifact[] GetItemsOfType(ItemType type)
    {
        return artifacts.Where(item => item.type == type).ToArray();
    }
}