using UnityEngine;

public class BossSkillTrigger : MonoBehaviour
{
    public GameObject[] gameObjects; 
    private int currentIndex = 0; 
    private bool isSkillActive = true; 

    void Start()
    {
        if (gameObjects == null || gameObjects.Length == 0)
        {
            Debug.LogError("Vui lòng thêm các GameObject vào mảng trong Inspector.");
            return;
        }

        foreach (GameObject obj in gameObjects)
        {
            obj.SetActive(false);
        }

        InvokeRepeating("ActivateNextObject", 10f, 10f);
    }

    public void EndSkill()
    {
        isSkillActive = false;

        // Tắt toàn bộ GameObject trong chuỗi
        foreach (GameObject obj in gameObjects)
        {
            obj.SetActive(false);
        }

        // Thêm code để tắt script hoặc làm bất kỳ điều gì bạn muốn khi kỹ năng kết thúc.
        // Ví dụ:
        // gameObject.SetActive(false); // Tắt script hoặc GameObject chứa script
    }

    void ActivateNextObject()
    {
        if (!isSkillActive)
        {
            CancelInvoke("ActivateNextObject");
            return;
        }

        foreach (GameObject obj in gameObjects)
        {
            obj.SetActive(false);
        }

        gameObjects[currentIndex].SetActive(true);

        currentIndex = (currentIndex + 1) % gameObjects.Length;
    }
}
