using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public Transform defenders;
    public Transform invaders;

    private Unit[] defenderUnits;
    private bool battling = false;
    private float time;

    public void StartBattle(Unit[] defenderUnits, Unit[] invaderUnits)
    {
        battling = true;
        this.defenderUnits = defenderUnits;
        time = Time.time;
        PlaceUnits(defenderUnits, defenders);
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
            SimulationManager.main.EndBattle(defenderUnits, Time.time - time);
        }
    }
}
