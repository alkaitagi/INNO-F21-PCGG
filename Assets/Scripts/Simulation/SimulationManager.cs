using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager main;
    public SimulationConfig config;

    private int generation;
    private List<BattleReport> reports = new List<BattleReport>();
    private Unit[] defenders;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        print($"Simulation started.");
        generation = 0;
        defenders = new Unit[config.defenderCount];
        NextGeneration();
    }

    private void NextGeneration()
    {
        if (++generation == config.generations)
        {
            print("Simulation ended.");
            return;
        }
        reports.Clear();
        print($"Generation {generation}.");

        var invaders = GenerateInvaders();
        for (int i = 0; i < config.population; i++)
        {
            var battle = Instantiate(config.battlefield, transform);
            var index = Mathf.RoundToInt((i + 1) / 2) * (i % 2 == 0 ? 1 : -1);
            battle.transform.localPosition = index * Vector2.up;
            battle.StartBattle(MutateDefenders(defenders), invaders);
        }
    }

    public void EndBattle(BattleReport report)
    {
        print("Battle ended.");
        reports.Add(report);
        if (reports.Count == config.population)
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);

            if (reports.Any(r => r.isWin))
            {
                defenders = reports
                    .Where(r => r.isWin)
                    .OrderBy(r => r.duration)
                    .First().units;
                print("Surviours are victorious.");
            }
            else
            {
                defenders = reports
                    .OrderBy(r => r.duration)
                    .Last().units;
                print("No survivors left.");
            }

            NextGeneration();
        }
    }

    private Unit[] MutateDefenders(Unit[] baseUnits)
    {
        var units = new Unit[baseUnits.Length];
        for (int i = 0; i < units.Length; i++)
        {
            var random = config.defenderUnits[Random.Range(0, config.defenderUnits.Length)];
            units[i] = baseUnits[i]
                ? Random.value < config.mutationChance
                    ? random
                    : baseUnits[i]
                : random;
        }
        return units;
    }

    private Unit[] GenerateInvaders()
    {
        var units = new Unit[config.invaderCount];
        for (int i = 0; i < config.invaderCount; i++)
        {
            // var perlin = Mathf.PerlinNoise(config.seed + generation + i, 0);
            // var index = Mathf.RoundToInt(perlin * config.invaderUnits.Length - 1);
            units[i] = config.invaderUnits[Random.Range(0, config.invaderUnits.Length)];
        }
        return units;
    }

}
