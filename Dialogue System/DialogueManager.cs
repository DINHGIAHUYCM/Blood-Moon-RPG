using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public GameObject[] dialogueObjects;
    public GameObject[] unlockObjects;
    public GameObject[] buttons;
    public GameObject endDialogueObject;

    private int currentIndex = 0;

    void Start()
    {
        // Ẩn tất cả đối tượng lời thoại và đối tượng mở khóa, chỉ hiển thị đối tượng và nút bấm đầu tiên
        HideAll(dialogueObjects);
        HideAll(unlockObjects);
        ShowObject(dialogueObjects[0]);
        ShowObject(unlockObjects[0]);
        SetButtonActive(buttons[0], true);
    }

    public void OnButtonClick(int buttonIndex)
    {
        // Kiểm tra xem có phải là nút bấm cuối cùng không
        if (buttonIndex == buttons.Length - 1)
        {
            // Kết thúc quá trình
            EndProcess();
            return;
        }

        // Tắt đối tượng lời thoại, đối tượng mở khóa và tất cả các nút bấm
        HideObject(dialogueObjects[currentIndex]);
        HideObject(unlockObjects[currentIndex]);
        SetAllButtonsActive(false);

        // Tăng chỉ số hiện tại để chuyển sang phần tiếp theo
        currentIndex++;

        // Kiểm tra xem đã vượt quá giới hạn mảng của đoạn hội thoại chưa
        if (currentIndex >= dialogueObjects.Length)
        {
            // Hiển thị Game Object kết thúc hội thoại
            ShowObject(endDialogueObject);
        }
        else
        {
            // Hiển thị đối tượng lời thoại, đối tượng mở khóa và nút bấm mới
            ShowObject(dialogueObjects[currentIndex]);
            ShowObject(unlockObjects[currentIndex]);
            SetButtonActive(buttons[currentIndex], true);
        }
    }

    private void EndProcess()
    {
        // Thực hiện bất kỳ hành động nào khi quá trình kết thúc
        // Debug.Log("Quá trình kết thúc.");
    }

    private void ShowObject(GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(true);
        }
    }

    private void HideObject(GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(false);
        }
    }

    private void SetButtonActive(GameObject button, bool active)
    {
        if (button != null)
        {
            button.SetActive(active);
        }
    }

    private void SetAllButtonsActive(bool active)
    {
        foreach (GameObject button in buttons)
        {
            SetButtonActive(button, active);
        }
    }

    private void HideAll(GameObject[] objects)
    {
        foreach (GameObject obj in objects)
        {
            HideObject(obj);
        }
    }
}