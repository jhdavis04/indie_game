using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class BattleManager : MonoBehaviour
{
    public TypeChart typeChart;
    public List<BaseCharacter> players, enemies;

    public void StartBattle()
    {
        foreach(var p in players) p.Initialize(p.data, true);
        foreach(var e in enemies) e.Initialize(e.data, false);
    }
}