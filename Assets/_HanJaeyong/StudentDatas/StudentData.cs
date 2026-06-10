using UnityEngine;

public enum StudentRole
{
    STRIKER,
    SPECIAL
}

[System.Serializable]
public struct BaseStatData
{
    public int maxHP;       // 최대 체력
    public int attack;      // 공격력
    public int defense;     // 방어력
    public int healing;     // 치유력
}

[System.Serializable]
public struct WeaponStatData
{
    public string weaponName;          // 무기 이름
    public Sprite weaponImage;         // 무기 이미지
    public int attack;                 // 무기 공격력
    public float critRate;             // 무기 치명타율
    public int armorPenetration;       // 무기 방어력 관통력
    public float range;                // 무기 사거리
}

[CreateAssetMenu(fileName = "NewStudentData", menuName = "Scriptable Objects/Student Data")]
public class StudentData : ScriptableObject
{
    [Header("[학생 정보]")]
    public string studentName;           // 학생 이름
    public bool isFavorite;              // 즐겨찾기 여부
    public int mysteryLevel;             // 신비 레벨
    public StudentRole role;             // 직업군 (STRIKER / SPECIAL)

    [Header("[학생 레벨]")]
    public int level;                    // 레벨
    public int exp;                      // 경험치

    [Header("[기본 능력치]")]
    public BaseStatData baseStats;       // 기본 능력치 그룹

    [Header("[무기 정보]")]
    public WeaponStatData weaponStats;   // 무기 정보 그룹
}