using UnityEngine;
[CreateAssetMenu(fileName = "Attack", menuName = "Character State/Attack")]
public class AttackData_SO : ScriptableObject
{
    //攻击范围
    public float attackRange;
    //技能范围
    public float skillRange;
    //CD
    public float coolDown;
    //攻击最小值
    public int minDamge;
    //攻击最大值
    public int maxDamge;
    //暴击加成百分比
    public float criticalMultiplier;
    //暴击率
    public float criticalChance;


    /// <summary>
    /// 更新攻击属性
    /// </summary>
    /// <param name="weapon_data"></param>
    public void ApplyWeaponData(AttackData_SO weapon_data)
    {
        attackRange = weapon_data.attackRange;
        coolDown = weapon_data.coolDown;
        minDamge = weapon_data.minDamge;
        maxDamge = weapon_data.maxDamge;
        criticalChance = weapon_data.criticalChance;
        criticalMultiplier = weapon_data.criticalMultiplier;
    }
}