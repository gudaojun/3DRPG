using UnityEngine;
[CreateAssetMenu(fileName = "Attack", menuName = "Character State/Attack")]
public class AttackData_SO : ScriptableObject
{
    //������Χ
    public float attackRange;
    //���ܷ�Χ
    public float skillRange;
    //CD
    public float coolDown;
    //������Сֵ
    public int minDamge;
    //�������ֵ
    public int maxDamge;
    //�����ӳɰٷֱ�
    public float criticalMultiplier;
    //������
    public float criticalChance;


    /// <summary>
    /// ���¹�������
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