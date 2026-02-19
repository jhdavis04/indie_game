using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewPlayerState", menuName = "RPG/Player State")]
public class PlayerState : ScriptableObject
{
    public int level = 1;
    public int statPointsAvailable;
    public int currentHL;

    public int power = 10, endurance = 10, agility = 10, chance = 10;

    public List<MoveData> equippedMoves = new List<MoveData>(new MoveData[6]);
    public List<MoveData> unlockedPool = new List<MoveData>();

    public void EquipMove(MoveData move, int slot)
    {
        if (equippedMoves.Contains(move)) equippedMoves[equippedMoves.IndexOf(move)] = null;
        equippedMoves[slot] = move;
    }
}