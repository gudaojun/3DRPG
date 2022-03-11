using UnityEngine;
[CreateAssetMenu(fileName = "New Data", menuName = "Character State/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Info")]

    public int maxHealth;

    public int currentHealth;

    public int baseDefence;

    public int currentDefence;
    [Header("DeadExp")]
    public int killPoint;
    [Header("Level")]

    public int currentLevel;

    public int MaxLevel;

    public int baseExp;

    public int currentExp;

    public float levelBuff;

    public float LevelMultiplier
    {
        get { return 1 + (currentLevel - 1) * levelBuff; }
    }

    public void UpdateExp(int point)
    {
        currentExp += point;
        if (currentExp > baseExp)
            LeveUp();
    }

    private void LeveUp()
    {
        currentLevel = Mathf.Clamp(currentLevel + 1, 0, MaxLevel);
        baseExp = (int)(baseExp * LevelMultiplier);
        maxHealth = (int)(maxHealth * LevelMultiplier);
        currentHealth = maxHealth;
        Debug.Log("Level up!");
    }
}