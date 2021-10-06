using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float range;
    public float damage;
    public float cooldown;
    public AttackEffect attackEffect;

    public bool isReady => cooldownTimer == 0;
    private float cooldownTimer;

    private void Update()
    {
        if (cooldownTimer > 0)
            cooldownTimer = Mathf.Max(0, cooldownTimer - Time.deltaTime);
    }

    public void Attack(Armor armor)
    {
        cooldownTimer = cooldown;
        armor.takeDamage(damage);
        Instantiate(attackEffect, transform.position, Quaternion.identity)
            .Launch(armor.transform.position);
    }
}
