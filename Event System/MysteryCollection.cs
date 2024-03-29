using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class MysteryCollection : MonoBehaviour
{
    public GameObject[] smallCells; // Mảng lưu trạng thái của 16 ô nhỏ
    public GameObject successPanel; // Panel thông báo thành công
    public TextMeshProUGUI diceCountText; // Text Mesh Pro để hiển thị số xúc sắc
    public TextMeshProUGUI dailyTaskCooldownText; // Text Mesh Pro để hiển thị thời gian nhiệm vụ hàng ngày

    private int openedCellsCount = 0;
    private List<int> unopenedCellIndices; // Danh sách chứa chỉ số của các ô chưa mở
    private bool loginTaskCompleted = false;
    private bool dailyTaskCompleted = false;
    private bool playGameTaskCompleted = false;
    private int diceCount = 0;
    private float dailyTaskCooldown = 10f; // Thời gian chờ cho nhiệm vụ hàng ngày
    private float timeUntilNextDailyTask = 0f;

    private const string PlayerPrefKey = "OpenedCells"; // Khóa để lưu trạng thái của các ô đã mở

    void Start()
    {
        successPanel.SetActive(false); // Ẩn panel thành công ban đầu

        // Khởi tạo danh sách các ô chưa mở
        unopenedCellIndices = new List<int>();
        for (int i = 0; i < smallCells.Length; i++)
        {
            unopenedCellIndices.Add(i);
        }

        // Kiểm tra nếu có trạng thái đã được lưu, thì khôi phục nó
        if (PlayerPrefs.HasKey(PlayerPrefKey))
        {
            string savedData = PlayerPrefs.GetString(PlayerPrefKey);
            List<int> savedIndices = new List<int>(System.Array.ConvertAll(savedData.Split(','), int.Parse));

            foreach (int index in savedIndices)
            {
                if (index >= 0 && index < smallCells.Length)
                {
                    OpenSmallCell(index);
                }
            }
        }

        // Kiểm tra trạng thái nhiệm vụ đã hoàn thành
        if (PlayerPrefs.HasKey("LoginTaskCompleted"))
        {
            loginTaskCompleted = PlayerPrefs.GetInt("LoginTaskCompleted") == 1;
        }
        if (PlayerPrefs.HasKey("DailyTaskCompleted"))
        {
            dailyTaskCompleted = PlayerPrefs.GetInt("DailyTaskCompleted") == 1;
        }
        if (PlayerPrefs.HasKey("PlayGameTaskCompleted"))
        {
            playGameTaskCompleted = PlayerPrefs.GetInt("PlayGameTaskCompleted") == 1;
        }

        // Kiểm tra thời gian còn lại cho nhiệm vụ hàng ngày
        if (PlayerPrefs.HasKey("TimeUntilNextDailyTask"))
        {
            timeUntilNextDailyTask = PlayerPrefs.GetFloat("TimeUntilNextDailyTask");
        }
    }

    void Update()
    {
        if (openedCellsCount == smallCells.Length)
        {
            // Nếu tất cả các ô đã được mở, hiển thị Panel thành công
            successPanel.SetActive(true);
        }

        // Hiển thị số xúc sắc
        diceCountText.text = "Total Dices: " + diceCount;

        // Kiểm tra nhiệm vụ hàng ngày
        if (!dailyTaskCompleted)
        {
            if (timeUntilNextDailyTask > 0)
            {
                // Nếu đang có thời gian đếm ngược
                if (Time.time >= timeUntilNextDailyTask)
                {
                    // Nếu đã đủ thời gian chờ, hiển thị nhiệm vụ
                    Debug.Log("Nhiệm vụ hàng ngày đã mở.");
                    dailyTaskCompleted = false;
                    PlayerPrefs.SetInt("DailyTaskCompleted", dailyTaskCompleted ? 1 : 0);
                    timeUntilNextDailyTask = 0;
                    PlayerPrefs.SetFloat("TimeUntilNextDailyTask", timeUntilNextDailyTask);
                    dailyTaskCooldownText.text = "Mission Unlocked!";
                }
                else
                {
                    // Cập nhật thời gian còn lại cho nhiệm vụ hàng ngày
                    float timeRemaining = timeUntilNextDailyTask - Time.time;
                    dailyTaskCooldownText.text = FormatTime(timeRemaining) + " until next mission.";
                }
            }
        }
    }

    private string FormatTime(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60);
        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void OpenSmallCell(int cellIndex)
    {
        if (smallCells[cellIndex].activeSelf)
        {
            // Nếu ô đang hiển thị, tắt nó (che hình ảnh)
            smallCells[cellIndex].SetActive(false);
            openedCellsCount++;

            // Lưu trạng thái của ô đã mở
            SaveOpenedCell(cellIndex);

            // Kiểm tra nhiệm vụ chơi game
            if (!playGameTaskCompleted)
            {
                PlayGameTask();
            }
        }
    }

    public void OpenRandomSmallCell()
    {
        if (diceCount >= 1 && unopenedCellIndices.Count > 0)
        {
            int randomIndex = Random.Range(0, unopenedCellIndices.Count);
            int randomCellIndex = unopenedCellIndices[randomIndex];
            OpenSmallCell(randomCellIndex);
            unopenedCellIndices.RemoveAt(randomIndex); // Loại bỏ ô đã mở khỏi danh sách
            diceCount -= 1; // Giảm số xúc sắc sau khi mở
        }
    }

    public void ResetGame()
    {
        // Đặt lại trò chơi về trạng thái ban đầu
        foreach (GameObject cell in smallCells)
        {
            cell.SetActive(true);
        }

        openedCellsCount = 0;
        unopenedCellIndices.Clear();

        for (int i = 0; i < smallCells.Length; i++)
        {
            unopenedCellIndices.Add(i);
        }

        // Xóa trạng thái đã lưu
        PlayerPrefs.DeleteKey(PlayerPrefKey);

        // Đặt lại trạng thái nhiệm vụ
        loginTaskCompleted = false;
        PlayerPrefs.SetInt("LoginTaskCompleted", loginTaskCompleted ? 1 : 0);

        dailyTaskCompleted = false;
        PlayerPrefs.SetInt("DailyTaskCompleted", dailyTaskCompleted ? 1 : 0);

        playGameTaskCompleted = false;
        PlayerPrefs.SetInt("PlayGameTaskCompleted", playGameTaskCompleted ? 1 : 0);

        timeUntilNextDailyTask = 0;
        PlayerPrefs.SetFloat("TimeUntilNextDailyTask", timeUntilNextDailyTask);

        // Đặt lại số xúc sắc
        diceCount = 0;
    }

    public void PlayGameTask()
    {
        // Hoàn thành nhiệm vụ chơi game
        playGameTaskCompleted = true;
        PlayerPrefs.SetInt("PlayGameTaskCompleted", playGameTaskCompleted ? 1 : 0);

        // Nhận xúc sắc
        diceCount += 1;

        //Debug.Log("Nhiệm vụ chơi game đã hoàn thành. Nhận 3 xúc sắc. Tổng xúc sắc: " + diceCount);

        // Cập nhật thời gian cho nhiệm vụ hàng ngày
        if (!dailyTaskCompleted)
        {
            dailyTaskCompleted = true;
            PlayerPrefs.SetInt("DailyTaskCompleted", dailyTaskCompleted ? 1 : 0);
            timeUntilNextDailyTask = Time.time + dailyTaskCooldown * 3600; // 10 giờ
            PlayerPrefs.SetFloat("TimeUntilNextDailyTask", timeUntilNextDailyTask);
        }
    }

    private void SaveOpenedCell(int cellIndex)
    {
        if (!PlayerPrefs.HasKey(PlayerPrefKey))
        {
            PlayerPrefs.SetString(PlayerPrefKey, cellIndex.ToString());
        }
        else
        {
            string savedData = PlayerPrefs.GetString(PlayerPrefKey);
            List<int> savedIndices = new List<int>(System.Array.ConvertAll(savedData.Split(','), int.Parse));
            savedIndices.Add(cellIndex);
            PlayerPrefs.SetString(PlayerPrefKey, string.Join(",", savedIndices.Select(x => x.ToString()).ToArray()));
        }
    }
}