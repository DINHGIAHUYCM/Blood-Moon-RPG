using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlotMachine : MonoBehaviour
{
    public Image[] slotImages;
    public Sprite[] rewardSprites;
    public GameObject[] rewardPanels;

    private bool isSpinning = false;

    private void Start()
    {
        foreach (var panel in rewardPanels)
        {
            panel.SetActive(false);
        }
    }

    public void Spin()
    {
        if (isSpinning)
            return;

        isSpinning = true;
        foreach (var panel in rewardPanels)
        {
            panel.SetActive(false);
        }

        StartCoroutine(SpinAnimation());
    }

    private IEnumerator SpinAnimation()
    {
        float animationDuration = 1.0f;
        float spinSpeed = 0.1f;
        float elapsedTime = 0.0f;

        while (elapsedTime < animationDuration)
        {
            for (int i = 0; i < slotImages.Length; i++)
            {
                int randomIndex = Random.Range(0, rewardSprites.Length);
                slotImages[i].sprite = rewardSprites[randomIndex];
            }

            yield return new WaitForSeconds(spinSpeed);
            elapsedTime += spinSpeed;
        }

        string[] spriteNames = new string[slotImages.Length];
        for (int i = 0; i < slotImages.Length; i++)
        {
            spriteNames[i] = slotImages[i].sprite.name;
        }

        CheckAndActivateReward(spriteNames);
        isSpinning = false;
    }

    private void CheckAndActivateReward(string[] spriteNames)
    {
        // Sort the sprite names to easily compare them
        System.Array.Sort(spriteNames);

        // Concatenate sorted sprite names to find a match
        string concatenatedNames = string.Join("", spriteNames);

        switch (concatenatedNames)
        {
            case "EventEventEvent":
                rewardPanels[0].SetActive(true);
                Debug.Log("Trúng 3/3 Event! Nhận Panel x10 Event Card");
                break;
            case "StandardStandardStandard":
                rewardPanels[1].SetActive(true);
                Debug.Log("Trúng 3/3 Standard! Nhận Panel x10 Standard Card");
                break;
            case "VIPVIPVIP":
                rewardPanels[2].SetActive(true);
                Debug.Log("Trúng 3/3 VIP! Nhận Panel x10 VIP Card");
                break;
            case "EventEventStandard":
                rewardPanels[3].SetActive(true);
                Debug.Log("Trúng 2/3 Event! Nhận Panel x1 Event Card");
                break;
            case "EventStandardEvent":
                rewardPanels[3].SetActive(true);
                Debug.Log("Trúng 2/3 Event! Nhận Panel x1 Event Card");
                break;
            case "StandardEventEvent":
                rewardPanels[3].SetActive(true);
                Debug.Log("Trúng 2/3 Event! Nhận Panel x1 Event Card");
                break;
            case "StandardStandardEvent":
                rewardPanels[4].SetActive(true);
                Debug.Log("Trúng 2/3 Standard! Nhận Panel x1 Standard Card");
                break;
            case "StandardEventStandard":
                rewardPanels[4].SetActive(true);
                Debug.Log("Trúng 2/3 Standard! Nhận Panel x1 Standard Card");
                break;
            case "EventStandardStandard":
                rewardPanels[4].SetActive(true);
                Debug.Log("Trúng 2/3 Standard! Nhận Panel x1 Standard Card");
                break;
            case "VIPVIPEvent":
                rewardPanels[5].SetActive(true);
                Debug.Log("Trúng 2/3 VIP! Nhận Panel x1 VIP Card");
                break;
            case "VIPEventVIP":
                rewardPanels[5].SetActive(true);
                Debug.Log("Trúng 2/3 VIP! Nhận Panel x1 VIP Card");
                break;
            case "EventVIPVIP":
                rewardPanels[5].SetActive(true);
                Debug.Log("Trúng 2/3 VIP! Nhận Panel x1 VIP Card");
                break;
            default:
                Debug.Log("Không trúng! Chia buồn!");
                break;
        }
    }
}