using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "RPG/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string characterName;
    public ElementType type;

    [Header("Base Stats (1-99)")]
    public int basePower;
    public int baseEndurance;
    public int baseAgility;
    public int baseChance;

    [Header("Growth Rates (For Enemies)")]
    public int powerGrowth;
    public int enduranceGrowth;
    public int agilityGrowth;
    public int chanceGrowth;

    [Header("Abilities")]
    public List<MoveData> moveList;
}