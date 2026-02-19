using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewSkillNode", menuName = "RPG/Skill Tree Node")]
public class SkillNode : ScriptableObject
{
    public string skillName;
    [TextArea] public string description;

    public MoveData moveReward;
    public int pointCost;

    [Header("Requirements")]
    public int requiredLevel;
    public List<SkillNode> requiredNodes;
    public int tokenCost = 1;
}