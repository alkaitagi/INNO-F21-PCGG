using UnityEngine;

public class Armor : MonoBehaviour
{
    public float maxHealth;
    public float health;
    public GameObject deathEffect;

    public void takeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (deathEffect)
                Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }

    }
}
