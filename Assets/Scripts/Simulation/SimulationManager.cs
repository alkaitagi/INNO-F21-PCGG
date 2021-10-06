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
        print($"Simulation started.");
        NextGeneration();
    }

    private void ResetGeneration()
    {
        generation = 0;
        army = new Army(config.armySize, config.initialMoney - config.moneyPerRound);
    }

    private void NextGeneration()
    {
        if (army == null || army.isDead) ResetGeneration();
        if (++generation == config.generations)
        {
            print("Simulation ended.");
            return;
        }
        experiment = 0;
        army.money += config.moneyPerRound;
        print($"Generation {generation}. Army worth: {army.worth}.");

        var invaders = GenerateInvaderUnits();
        for (int i = 0; i < config.population; i++)
        {
            var battle = Instantiate(config.battlefield, transform);
            var index = Mathf.RoundToInt((i + 1) / 2) * (i % 2 == 0 ? 1 : -1);
            battle.transform.localPosition = index * Vector2.up;
            battle.StartBattle(MutateDefenderArmy(army), invaders);
        }
        army = null;
    }

    public void EndBattle(Army army)
    {
        print("Battle ended.");
        if (this.army == null || army.worth > this.army.worth)
            this.army = army;
        if (++experiment == config.population)
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);
            NextGeneration();
        }
    }

    private Army MutateDefenderArmy(Army baseArmy)
    {
        var army = new Army(baseArmy.units.Length, baseArmy.money);
        for (int i = 0; i < baseArmy.units.Length; i++)
        {
            if (baseArmy.units[i])
                army.units[i] = baseArmy.units[i];
            else
            {
                var available = config.defenderUnits
                    .Where(u => u.price <= army.money)
                    .ToArray();
                if (available.Length > 0 && Random.value < 0.7)
                {
                    army.units[i] = available[Random.Range(0, available.Length)];
                    army.money -= army.units[i].price;
                }
            }
        }
        return army;
    }

    private Unit[] GenerateInvaderUnits()
    {
        var log = Mathf.Log10(1 + generation);
        var size = Mathf.RoundToInt(log * config.countLogScale);
        print($"Invader count: {size}.");
        var units = new Unit[size];
        for (int i = 0; i < size; i++)
        {
            var perlin = Mathf.PerlinNoise(config.seed + generation + i + log, 0);
            var random = Mathf.Clamp01(perlin) - 0.0001f;
            var index = Mathf.FloorToInt(random * config.invaderUnits.Length);
            units[i] = config.invaderUnits[index];
        }
        return units.ToArray();
    }

}
