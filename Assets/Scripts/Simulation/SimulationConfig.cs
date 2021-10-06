using UnityEngine;

[CreateAssetMenu(menuName = "Simulation Config")]
public class SimulationConfig : ScriptableObject
{
    [Header("Selection")]
    public int population;
    public int generations;
    public float mutationChance;

    [Header("Battle")]
    public float seed;
    public BattleManager battlefield;

    [Header("Defenders")]
    public int defenderCount;
    public Unit[] defenderUnits;

    [Header("Invaders")]
    public int invaderCount;
    public Unit[] invaderUnits;
}
