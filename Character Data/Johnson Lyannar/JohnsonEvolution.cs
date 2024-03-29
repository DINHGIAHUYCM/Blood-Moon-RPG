using UnityEngine;
using Kryz.CharacterStats.Examples;

public class JohnsonEvolution : MonoBehaviour
{
    public Character character;
    public float healPercent = 0.02f;

    public void JohnsonHeal()
    {
        // Đọc giá trị PlayerPrefs
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel_JohnsonLyannar", 1);

        // Nếu currentLevel = 2, tăng healPercent gấp đôi
        if (currentLevel == 2 || currentLevel == 3 || currentLevel == 4|| currentLevel == 5|| currentLevel == 6 )
        {
            healPercent = 0.04f;
        }

        float maxHP = character.HP.BaseValue;
        float currentHP = character.CurrentHP;

        float healAmount = maxHP * healPercent;
        healAmount = Mathf.Round(healAmount);

        character.CurrentHP = Mathf.Min(maxHP, currentHP + healAmount);
    }
}
