using System.Linq;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager main;
    public SimulationConfig config;

    private int generation;
    private int experiment;
    private Army army;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        generation = 0;
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
        for (int i = 0; i < config.population; i++)
        {
            var battle = Instantiate(config.battlefield, transform);
            var index = Mathf.RoundToInt((i + 1) / 2) * (i % 2 == 0 ? 1 : -1);
            battle.transform.localPosition = index * Vector2.up;
            battle.StartBattle(MutateDefenderArmy(army), invaders);
        }
    }

    public void EndBattle(Army army)
    {
        print("Battle ended.");
        if (army.worth > this.army.worth)
            this.army = army;
        if (++experiment == config.population)
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);
            NextGeneration();
        }
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
        var size = (int)(Mathf.Log10(1 + generation) * 10);
        print($"Invaders count: {size}.");
        var units = new Unit[size];
        for (int i = 0; i < units.Length; i++)
            units[i] = config.invaderUnits[Random.Range(0, config.invaderUnits.Length)];
        return units.ToArray();
    }

}
