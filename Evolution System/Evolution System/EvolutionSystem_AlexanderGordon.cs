using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EvolutionSystem_AlexanderGordon : MonoBehaviour
{
    public GameObject[] evoStars; // Mảng các GameObject thể hiện các cấp độ tiến hóa
    public GameObject[] evoButtons; // Mảng các GameObject thể hiện các nút tiến hóa
    public TextMeshProUGUI levelText; // TextMeshPro để hiển thị cấp độ

    private int currentLevel = 1; // Cấp độ hiện tại

    private const string LevelKey = "CurrentLevel_AlexanderGordon"; // Khóa lưu trữ cấp độ trong PlayerPrefs

    private void Start()
    {
        // Khởi tạo cấp độ từ PlayerPrefs
        if (PlayerPrefs.HasKey(LevelKey))
        {
            currentLevel = PlayerPrefs.GetInt(LevelKey);
        }

        // Cập nhật hiển thị cấp độ
        UpdateLevelText();

        // Đặt trạng thái ban đầu cho các cấp độ tiến hóa và nút tiến hóa
        SetEvolutionState();
    }

    public void UpgradeLevel()
    {
        // Kiểm tra nếu cấp độ hiện tại chưa đạt tới cấp độ cuối cùng
        if (currentLevel < 6)
        {
            // Tăng cấp độ lên 1 đơn vị
            currentLevel++;

            // Lưu cấp độ vào PlayerPrefs
            PlayerPrefs.SetInt(LevelKey, currentLevel);

            // Cập nhật hiển thị cấp độ
            UpdateLevelText();

            // Đặt trạng thái cho các cấp độ tiến hóa và nút tiến hóa
            SetEvolutionState();
        }
    }

    public void ResetLevel()
    {
        // Đặt cấp độ về 1
        currentLevel = 1;

        // Xóa lưu trữ cấp độ trong PlayerPrefs
        PlayerPrefs.DeleteKey(LevelKey);

        // Cập nhật hiển thị cấp độ
        UpdateLevelText();

        // Đặt trạng thái ban đầu cho tất cả các cấp độ tiến hóa và nút tiến hóa
        foreach (GameObject star in evoStars)
        {
            star.SetActive(false);
        }

        foreach (GameObject button in evoButtons)
        {
            button.SetActive(false);
        }

        // Kích hoạt cấp độ tiến hóa và nút tiến hóa đầu tiên
        if (evoStars.Length > 0 && evoButtons.Length > 0)
        {
            evoStars[0].SetActive(true);
            evoButtons[0].SetActive(true);
        }
    }

    private void UpdateLevelText()
    {
        // Hiển thị cấp độ lên TextMeshPro
        levelText.text = "E" + currentLevel.ToString();
    }

    private void SetEvolutionState()
    {
        // Vô hiệu hóa tất cả các cấp độ tiến hóa và nút tiến hóa
        foreach (GameObject star in evoStars)
        {
            star.SetActive(false);
        }

        foreach (GameObject button in evoButtons)
        {
            button.SetActive(false);
        }

        // Kích hoạt cấp độ tiến hóa và nút tiến hóa tương ứng với cấp độ hiện tại
        if (currentLevel <= evoStars.Length && currentLevel <= evoButtons.Length)
        {
            for (int i = 0; i < currentLevel; i++)
            {
                evoStars[i].SetActive(true);
                evoButtons[i].SetActive(true);
            }
        }
    }
}