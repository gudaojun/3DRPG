using System;
using UnityEngine;

public class CharacterState : MonoBehaviour
{
    public CharacterData_SO templateData;
    public CharacterData_SO characterData;
    public AttackData_SO attackData;
    private AttackData_SO baseAttackData;
    private RuntimeAnimatorController baseAnimator;
    [HideInInspector]
    public bool isCritical;//是否暴击
    [Header("武器")]
    public Transform weaponslot;
    public event Action<int, int> UpdateHealthBarOnAttack;
    #region 拿到数据
    public int MaxHealth
    {
        get { if (characterData != null) return characterData.maxHealth; else return 0; }
        set { characterData.maxHealth = value; }
    }

    public int CurrentHealth
    {
        get { if (characterData != null) return characterData.currentHealth; else return 0; }
        set { characterData.currentHealth = value; }
    }

    public int BaseDefence
    {
        get { if (characterData != null) return characterData.baseDefence; else return 0; }
        set { characterData.baseDefence = value; }
    }

    public int CurrentDefence
    {
        get { if (characterData != null) return characterData.currentDefence; else return 0; }
        set { characterData.currentDefence = value; }
    }
    #endregion
    private void Awake()
    {
        if (templateData != null)
            characterData = Instantiate(templateData);
        baseAttackData = Instantiate(attackData);
        baseAnimator = GetComponent<Animator>().runtimeAnimatorController;
    }

    public void ApplyHealth(int health)
    {
        if (CurrentHealth + health <= MaxHealth)
        {
            CurrentHealth += health;
        }
        else
            CurrentHealth = MaxHealth;
    }

    #region 攻击
    public void TakeDamage(CharacterState attacker, CharacterState defener)
    {
        int damage = Mathf.Max(attacker.CurrentDamage() - defener.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);
        if (attacker.isCritical)
        {
            defener.gameObject.GetComponent<Animator>().SetTrigger("Hit");
        }
        // 更新到UI
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
        //死亡升级经验
        if (CurrentHealth <= 0)
        {
            attacker.characterData.UpdateExp(characterData.killPoint);
        }
    }
    public void TakeDamage(int damage, CharacterState defener)
    {
        int currentdamage = Mathf.Max(damage - defener.CurrentDefence, 0);
        CurrentHealth = Mathf.Max(CurrentHealth - currentdamage, 0);
        UpdateHealthBarOnAttack?.Invoke(CurrentHealth, MaxHealth);
        GameManager.Instance.playerState.characterData.UpdateExp(characterData.killPoint);
    }

    private int CurrentDamage()
    {
        float coreDamage = UnityEngine.Random.Range(attackData.minDamge, attackData.maxDamge);
        if (isCritical)
        {
            coreDamage *= attackData.criticalMultiplier;
        }
        return (int)coreDamage;
    }
    #endregion
    #region 装备武器
    public void ChengeWeapon(ItemData_SO weapon)
    {
        UnEquipWeapon();
        EquidWeapon(weapon);
    }

    public void EquidWeapon(ItemData_SO itemData_SO)
    {
        if (itemData_SO.prefab != null)
            Instantiate(itemData_SO.prefab, weaponslot);

        //TODO更新属性
        attackData.ApplyWeaponData(itemData_SO.attackData);
        GetComponent<Animator>().runtimeAnimatorController = itemData_SO.weaponAnimator;
        //   InventoryManager.Instance.UpdateStateText(MaxHealth, attackData.minDamge, attackData.maxDamge);
    }
    public void UnEquipWeapon()
    {
        if (weaponslot.transform.childCount != 0)
        {
            for (int i = 0; i < weaponslot.transform.childCount; i++)
            {
                Destroy(weaponslot.transform.GetChild(i).gameObject);
            }
        }
        attackData.ApplyWeaponData(baseAttackData);
        GetComponent<Animator>().runtimeAnimatorController = baseAnimator;
    }
    #endregion
}