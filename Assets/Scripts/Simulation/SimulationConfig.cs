using UnityEngine;

[CreateAssetMenu(menuName = "Simulation Config")]
public class SimulationConfig : ScriptableObject
{
    [Header("Simulation")]
    public float seed;
    public int population;
    public int generations;

    [Header("Defenders")]
    public int armySize;
    public Unit[] defenderUnits;

    [Header("Invaders")]
    public int statsScale;
    public Unit[] invaderUnits;

    [Header("Economy")]
    public int initialMoney;
    public int moneyPerRound;
}
