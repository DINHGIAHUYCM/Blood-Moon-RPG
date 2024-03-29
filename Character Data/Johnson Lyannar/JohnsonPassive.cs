using UnityEngine;
using Kryz.CharacterStats.Examples;
using TMPro;
using System.Collections;

public class JohnsonPassive : MonoBehaviour
{
    public JohnsonSkill1 skill1;
    public JohnsonSkill2 skill2;
    public JohnsonSkill3 skill3;
    public Character character;
    public TextMeshProUGUI stackText;
    public GameObject[] stackObjects;    // Mảng chứa các Stack Object

    [Header("Cooldown Times")]
    [SerializeField]
    private float cooldownTimeI = 4f; // Cooldown time for KeyCode.I
    [SerializeField]
    private float cooldownTimeO = 6f; // Cooldown time for KeyCode.O
    [SerializeField]
    private float cooldownTimeP = 30f; // Cooldown time for KeyCode.P

    private int passivePoints = 0;
    private const int maxPassivePoints = 10;
    private float healPercentage = 0.2f;
    private bool isHealing = false;

     private void Start()
    {
        int currentLevel = PlayerPrefs.GetInt("CurrentLevel_JohnsonLyannar", 1);
        if (currentLevel > 1)
        {
            healPercentage = 0.3f;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.O) || Input.GetKeyDown(KeyCode.P))
        {
            IncreasePassivePoints();
        }

        UpdateStackText();

        if (passivePoints >= maxPassivePoints && !isHealing)
        {
            if (CheckSkillCooldowns())
            {
                StartCoroutine(HealCharacterCoroutine());
            }
        }
    }

    private void IncreasePassivePoints()
    {
        passivePoints++;

        if (Input.GetKeyDown(KeyCode.I))
        {
            StartCoroutine(StartCooldown(cooldownTimeI));
        }
        else if (Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(StartCooldown(cooldownTimeO));
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(StartCooldown(cooldownTimeP));
        }

        // Bật/tắt Stack Object theo thứ tự
        for (int i = 0; i < stackObjects.Length; i++)
        {
            if (i < passivePoints)
            {
                stackObjects[i].SetActive(true);
            }
            else
            {
                stackObjects[i].SetActive(false);
            }
        }
    }

    private IEnumerator StartCooldown(float cooldownTime)
    {
        yield return new WaitForSeconds(cooldownTime);
    }

    public void Add1Point()
    {
        if (passivePoints < maxPassivePoints)
        {
            passivePoints++;
            UpdateStackText();

            // Bật/tắt Stack Object theo thứ tự
            for (int i = 0; i < stackObjects.Length; i++)
            {
                if (i < passivePoints)
                {
                    stackObjects[i].SetActive(true);
                }
                else
                {
                    stackObjects[i].SetActive(false);
                }
            }
        }
    }

    private void ResetPassivePoints()
    {
        passivePoints = 0;

        // Tắt tất cả Stack Object
        foreach (GameObject stackObject in stackObjects)
        {
            stackObject.SetActive(false);
        }
    }

    private void UpdateStackText()
    {
        if (stackText != null)
        {
            stackText.text = $"{passivePoints}";
        }
    }

    private bool CheckSkillCooldowns()
    {
        bool skill1Cooldown = skill1 == null || skill1.IsOnCooldown();
        bool skill2Cooldown = skill2 == null || skill2.IsOnCooldown();

        return !skill1Cooldown && !skill2Cooldown;
    }

    private IEnumerator HealCharacterCoroutine()
    {
        isHealing = true;

        float maxHP = character.HP.BaseValue;
        float currentHP = character.CurrentHP;

        float healAmount = maxHP * healPercentage;
       healAmount = Mathf.Round(healAmount);

        character.CurrentHP = Mathf.Min(maxHP, currentHP + healAmount);

        yield return new WaitForSeconds(0.1f); // Add a small wait time if needed

        ResetPassivePoints();
        isHealing = false;
    }
}