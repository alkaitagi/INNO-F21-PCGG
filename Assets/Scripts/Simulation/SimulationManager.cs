using System.Linq;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager main;
    public SimulationConfig config;

    private int generation;
    private int experiment;
    private Army army;

    private BattleManager[] battles;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        generation = 0;
        battles = Enumerable
            .Range(0, config.population)
            .Select(i => Instantiate(
                config.battlefield,
                3 * i * Vector2.up,
                Quaternion.identity))
            .ToArray();
        army = new Army(config.armySize, config.initialMoney);
        print($"Simulation started.");
        NextGeneration();
    }

    private void NextGeneration()
    {
        if (++generation == config.generations)
        {
            print("Simulation ended.");
            return;
        }
        print($"Round {generation}.");
        experiment = 0;
        var invaders = GenerateInvaderUnits();
        foreach (var battle in battles)
            battle.StartBattle(MutateDefenderArmy(army), invaders);
    }

    public void EndBattle(Army army)
    {
        if (army.worth > this.army.worth)
            this.army = army;
        if (++experiment == config.population)
            NextGeneration();
    }

    private Army MutateDefenderArmy(Army army)
    {
        var newArmy = new Army(army.units.Length, army.money);
        for (int i = 0; i < army.units.Length; i++)
        {
            if (army.units[i])
                newArmy.units[i] = army.units[i];
            else
            {
                var available = config.defenderUnits
                    .Where(u => u.price <= army.money)
                    .ToArray();
                if (available.Length > 0)
                {
                    newArmy.units[i] = available[Random.Range(0, available.Length)];
                    newArmy.money -= newArmy.units[i].price;
                }
            }
        }
        return newArmy;
    }

    private Unit[] GenerateInvaderUnits()
    {
        var size = (int)Mathf.Log10(1 + generation) * 10;
        var units = new Unit[size];
        for (int i = 0; i < units.Length; i++)
        {
            units[i] = config.invaderUnits[Random.Range(0, config.invaderUnits.Length)];
        }
        return units.ToArray();
    }

}
