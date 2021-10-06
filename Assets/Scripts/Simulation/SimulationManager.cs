using System.Linq;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public static SimulationManager main;
    public SimulationConfig config;

    private int generation;
    private int experiment;
    private float currentBestTime;
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
        currentBestTime = float.PositiveInfinity;
        experiment = 0;
        print($"Generation {generation}.");

        var invaders = GenerateInvaderUnits();
        for (int i = 0; i < config.population; i++)
        {
            var battle = Instantiate(config.battlefield, transform);
            var index = Mathf.RoundToInt((i + 1) / 2) * (i % 2 == 0 ? 1 : -1);
            battle.transform.localPosition = index * Vector2.up;
            battle.StartBattle(defenders, invaders);
        }
    }

    public void EndBattle(Unit[] defenders, float time)
    {
        print("Battle ended.");
        if (time < currentBestTime)
        {
            currentBestTime = time;
            this.defenders = defenders;
        }
        if (++experiment == config.population)
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);
            NextGeneration();
        }
    }

    private Unit[] MutateDefenderArmy(Unit[] baseUnits)
    {
        var units = new Unit[baseUnits.Length];
        for (int i = 0; i < units.Length; i++)
            if (Random.value < 0.7)
                units[i] = Random.value < config.mutationChance
                    ? config.defenderUnits[Random.Range(0, config.defenderUnits.Length)]
                    : baseUnits[i];
        return units;
    }

    private Unit[] GenerateInvaderUnits()
    {
        var units = new Unit[config.invaderCount];
        for (int i = 0; i < config.invaderCount; i++)
        {
            var perlin = Mathf.PerlinNoise(config.seed + generation + i, 0);
            var random = Mathf.Clamp01(perlin) - 0.0001f;
            var index = Mathf.FloorToInt(random * config.invaderUnits.Length);
            units[i] = config.invaderUnits[index];
        }
        return units;
    }

}
