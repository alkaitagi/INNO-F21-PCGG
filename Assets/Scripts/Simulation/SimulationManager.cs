using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class SimulationManager : MonoBehaviour
{
    public SimulationConfig config;

    [Header("Teams")]
    public Transform defenders;
    public Transform invaders;

    private int generation;
    private Army army;

    private void Start()
    {
        generation = 1;
        army = new Army()
        {
            units = Enumerable
                .Range(0, config.armySize)
                .Select(_ => Random.Range(0, config.defenderUnits.Length))
                .Select(i => config.defenderUnits[i])
                .ToArray(),
            money = config.initialMoney - config.moneyPerRound,
        };
        print($"Simulation started.");
        NextGeneration();
    }

    private void NextGeneration()
    {
        if (++generation == config.generations)
        {
            print("Simulation ended.");
        }

        army.money += config.moneyPerRound;
        PlaceUnits(army.units, defenders);
        PlaceUnits(army.units, invaders, defenders);
    }

    private void PlaceUnits(Unit[] units, Transform parent, Transform target = null)
    {
        for (int i = 0; i < units.Length; i++)
        {
            var instance = Instantiate(units[i], parent);
            instance.transform.localPosition = 0.5f * i * Vector3.right;
            if (target
                && instance.GetComponent<Movement>() is Movement movement)
                movement.target = target;
        }
    }

    private void FixedUpdate()
    {
        if (defenders.childCount * invaders.childCount == 0)
        {
            print("End of round.");
        }
    }

}
