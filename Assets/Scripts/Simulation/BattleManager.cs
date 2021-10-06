using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public Transform defenders;
    public Transform invaders;

    private Unit[] survivors;
    private Army army;

    private bool battling = false;

    public void StartBattle(Army defenderArmy, Unit[] invaderUnits)
    {
        battling = true;
        army = defenderArmy;
        survivors = PlaceUnits(defenderArmy.units, defenders);
        PlaceUnits(invaderUnits, invaders, defenders);
    }

    private Unit[] PlaceUnits(Unit[] units, Transform parent, Transform target = null)
    {
        var instances = new Unit[units.Length];
        for (int i = 0; i < units.Length; i++)
        {
            if (!units[i]) continue;
            var instance = Instantiate(units[i], parent);
            instance.transform.localPosition = 0.5f * i * Vector3.right;
            if (target
                && instance.GetComponent<Movement>() is Movement movement)
                movement.target = target;
            instances[i] = instance;
        }
        return instances;
    }

    private void FixedUpdate()
    {
        if (battling && defenders.childCount * invaders.childCount == 0)
        {
            battling = false;
            for (int i = 0; i < survivors.Length; i++)
            {
                if (!survivors[i])
                    army.units[i] = null;
            }
            SimulationManager.main.EndBattle(army);
        }
    }
}
