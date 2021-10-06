using UnityEngine;

public class Unit : MonoBehaviour
{
    private Armor armor;
    private Movement movement;
    private Weapon weapon;

    public Team team;
    public SpriteRenderer flag;

    private void Awake()
    {
        armor = GetComponent<Armor>();
        movement = GetComponent<Movement>();
        weapon = GetComponent<Weapon>();
    }

    private void Start()
    {
        if (flag)
            flag.color = team.color;
    }

    private void Update()
    {
        if (weapon?.isReady == true)
            Attack();
    }

    private bool Attack()
    {
        var colliders = Physics2D.OverlapAreaAll(
             transform.position + Vector3.left * weapon.range,
             transform.position + Vector3.right * weapon.range);

        Armor target = null;
        foreach (var collider in colliders)
        {
            var unit = collider.GetComponent<Unit>();
            if (unit?.team != team && unit.armor)
            {
                if (!target ||
                    (collider.transform.position - transform.position).sqrMagnitude <
                    (target.transform.position - transform.position).sqrMagnitude)
                    target = unit.armor;
            }
        }
        if (target)
        {
            weapon.Attack(target);
            return true;
        }
        return false;
    }
}
