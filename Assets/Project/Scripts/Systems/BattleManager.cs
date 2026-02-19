using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public enum BattleState { Start, PlayerTurn, EnemyTurn, Busy, Won, Lost }

public class BattleManager : MonoBehaviour
{
    [Header("Global Data")]
    public PlayerState globalPlayerState;
    public TypeChart typeChart;

    [Header("Battle State")]
    public BattleState currentState;
    
    [Header("Scene References")]
    public List<BaseCharacter> playerBattleSlots; 
    public List<BaseCharacter> enemyBattleSlots; 
    
    private List<BaseCharacter> turnOrder = new List<BaseCharacter>();
    private int turnIndex = 0;

    void Start() => StartCoroutine(SetupBattle());

    #region Battle Flow
    IEnumerator SetupBattle()
    {
        currentState = BattleState.Start;

        for (int i = 0; i < playerBattleSlots.Count; i++)
        {
            if (i < globalPlayerState.allCharacters.Count)
            {
                playerBattleSlots[i].gameObject.SetActive(true);
                playerBattleSlots[i].Initialize(true, globalPlayerState.allCharacters[i], null);
            }
            else
            {
                playerBattleSlots[i].gameObject.SetActive(false);
            }
        }

        foreach (var enemy in enemyBattleSlots)
        {
            if (enemy.enemyData != null)
                enemy.Initialize(false, null, enemy.enemyData);
        }

        yield return new WaitForSeconds(1f);

        DetermineTurnOrder();
        StartCoroutine(NextTurn());
    }

    void DetermineTurnOrder()
    {
        turnOrder.Clear();
        
        var allUnits = Object.FindObjectsByType<BaseCharacter>(FindObjectsSortMode.None)
                     .Where(u => u.currentHL > 0);
                    
        turnOrder = allUnits.OrderByDescending(u => u.GetEffectiveSpeed()).ToList();
        
        Debug.Log("Turn Order Determined: " + string.Join(", ", turnOrder.Select(u => u.characterName)));
    }

    IEnumerator NextTurn()
    {
        if (enemyBattleSlots.All(e => e.currentHL <= 0)) { StartCoroutine(EndBattle(true)); yield break; }
        if (playerBattleSlots.Where(p => p.gameObject.activeSelf).All(p => p.currentHL <= 0)) { StartCoroutine(EndBattle(false)); yield break; }
        BaseCharacter activeUnit = turnOrder[turnIndex];

        if (activeUnit.currentHL <= 0)
        {
            AdvanceTurn();
            yield break;
        }

        if (activeUnit.isPlayer)
        {
            currentState = BattleState.PlayerTurn;
            Debug.Log($"Waiting for {activeUnit.characterName}'s input...");
        }
        else
        {
            currentState = BattleState.EnemyTurn;
            yield return new WaitForSeconds(1f);
            ExecuteEnemyAI(activeUnit);
        }
    }

    void AdvanceTurn()
    {
        turnIndex++;
        if (turnIndex >= turnOrder.Count)
        {
            turnIndex = 0;
            DetermineTurnOrder();
        }
        StartCoroutine(NextTurn());
    }
    #endregion

    #region Combat Actions
    public void OnPlayerMoveSelected(MoveData move)
    {
        if (currentState != BattleState.PlayerTurn) return;
        BaseCharacter target = enemyBattleSlots.FirstOrDefault(e => e.currentHL > 0);
        
        if (target != null)
            StartCoroutine(ExecuteMove(turnOrder[turnIndex], target, move));
    }

    void ExecuteEnemyAI(BaseCharacter enemy)
    {
        MoveData move = enemy.enemyData.moveList[Random.Range(0, enemy.enemyData.moveList.Count)];
        var targets = playerBattleSlots.Where(p => p.gameObject.activeSelf && p.currentHL > 0).ToList();
        BaseCharacter target = targets[Random.Range(0, targets.Count)];
        StartCoroutine(ExecuteMove(enemy, target, move));
    }

    IEnumerator ExecuteMove(BaseCharacter attacker, BaseCharacter defender, MoveData move)
    {
        currentState = BattleState.Busy;
        Debug.Log($"{attacker.characterName} uses {move.moveName} on {defender.characterName}!");
        move.Execute(attacker, defender, typeChart, 1);
        yield return new WaitForSeconds(1.5f);
        AdvanceTurn();
    }

    IEnumerator EndBattle(bool won)
    {
        currentState = won ? BattleState.Won : BattleState.Lost;
        Debug.Log(won ? "Victory!" : "Defeat...");

        if (won)
        {
            foreach (var p in playerBattleSlots)
            {
                if (p.gameObject.activeSelf) p.SyncToSave();
            }
        }

        yield return null;
    }
    #endregion
}