using UnityEngine;
using TMPro;

public class ShowCharacterEvolutionLevel : MonoBehaviour
{
    public TextMeshProUGUI evoStateText; // Đối tượng TextMeshPro để hiển thị trạng thái tiến hóa

    private const string LevelKey = "CurrentLevel_JohnsonLyannar"; // Khóa lưu trữ cấp độ trong PlayerPrefs

    private void Start()
    {
        // Kiểm tra nếu tồn tại giá trị cấp độ trong PlayerPrefs
        if (PlayerPrefs.HasKey(LevelKey))
        {
            // Đọc giá trị cấp độ từ PlayerPrefs
            int currentLevel = PlayerPrefs.GetInt(LevelKey);

            // Hiển thị trạng thái tiến hóa
            ShowEvolutionState(currentLevel);
        }
        else
        {
            // Hiển thị trạng thái tiến hóa mặc định
            ShowEvolutionState(1);
        }
    }

    public void Update(){
        if (PlayerPrefs.HasKey(LevelKey))
        {
            // Đọc giá trị cấp độ từ PlayerPrefs
            int currentLevel = PlayerPrefs.GetInt(LevelKey);

            // Hiển thị trạng thái tiến hóa
            ShowEvolutionState(currentLevel);
        }
        else
        {
            // Hiển thị trạng thái tiến hóa mặc định
            ShowEvolutionState(1);
        }
    }

    private void ShowEvolutionState(int level)
    {
        // Hiển thị trạng thái tiến hóa lên TextMeshPro
        evoStateText.text = "E" + level.ToString();
    }
}