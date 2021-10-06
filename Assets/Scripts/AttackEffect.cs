using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public float speed;
    private Vector3 target;

    private void Start()
    {
        transform.up = (target - transform.position);
    }

    public void Launch(Vector3 target)
    {
        this.target = (Vector2)target + (Random.insideUnitCircle * 0.25f);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            target,
            speed * Time.deltaTime);
    }
}
