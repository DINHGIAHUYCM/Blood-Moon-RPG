using UnityEngine;
using UnityEngine.UI;

public class ShowEvolutionDetail : MonoBehaviour
{
    public GameObject[] evolutionObjects;
    public Button[] evolutionButtons;

    private int currentEvolutionIndex = 0;

    private void Start()
    {
        ActivateEvolutionObject(currentEvolutionIndex);

        // Gán các sự kiện onClick cho các nút bấm
        for (int i = 0; i < evolutionButtons.Length; i++)
        {
            int index = i; // Lưu giá trị index vào biến tạm để sử dụng trong sự kiện
            evolutionButtons[i].onClick.AddListener(() => OnEvolutionButtonClicked(index));
        }
    }

    private void OnEvolutionButtonClicked(int index)
    {
        // Tắt GameObject hiện tại
        DeactivateEvolutionObject(currentEvolutionIndex);

        // Kích hoạt GameObject mới
        ActivateEvolutionObject(index);

        // Cập nhật currentEvolutionIndex
        currentEvolutionIndex = index;
    }

    private void ActivateEvolutionObject(int index)
    {
        if (index >= 0 && index < evolutionObjects.Length)
        {
            evolutionObjects[index].SetActive(true);
        }
    }

    private void DeactivateEvolutionObject(int index)
    {
        if (index >= 0 && index < evolutionObjects.Length)
        {
            evolutionObjects[index].SetActive(false);
        }
    }
}