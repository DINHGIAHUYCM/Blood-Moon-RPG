using System.Collections;
using UnityEngine;

public class BossAnimator : MonoBehaviour
{
    public Animator bossAnimator;
    public float skillInterval = 6f; // Khoảng thời gian giữa các kỹ năng

    private string[] skillTriggers = { "C1", "C2", "C3", "C4", "C5", "C6" };
    private int currentSkillIndex = 0;

    private bool isAnimatorActive = true; // Biến kiểm soát trạng thái hoạt động của Animator
    private bool isCoroutineActive = true; // Biến kiểm soát trạng thái hoạt động của Coroutine

    private void Start()
    {
        // Kích hoạt kỹ năng C1 ngay từ đầu
        bossAnimator.SetTrigger(skillTriggers[currentSkillIndex]);

        // Bắt đầu Coroutine để thực hiện việc trigger các kỹ năng sau mỗi khoảng thời gian
        StartCoroutine(PlaySkills());
    }

    public void EndAnimator()
    {
        // Tắt Coroutine và ngừng chuyển đổi giữa các kỹ năng
        isAnimatorActive = false;
        isCoroutineActive = false;
    }

    IEnumerator PlaySkills()
    {
        while (isAnimatorActive && isCoroutineActive)
        {
            yield return new WaitForSeconds(skillInterval);

            // Chuyển sang kỹ năng tiếp theo
            currentSkillIndex = (currentSkillIndex + 1) % skillTriggers.Length;

            // Trigger kỹ năng hiện tại
            bossAnimator.SetTrigger(skillTriggers[currentSkillIndex]);
        }
    }

}
