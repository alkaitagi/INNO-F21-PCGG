using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public float speed;
    public Transform target;
    public float stoppingDistance;

    private new Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (target)
            Rotate();
        else
            transform.up = Vector2.right;
    }

    private void FixedUpdate()
    {
        if (target)
        {
            Rotate();
            if (CanMove())
            {
                Vector2 move = Time.fixedDeltaTime * speed * transform.up;
                rigidbody.MovePosition(rigidbody.position + move);
            }
        }
    }

    private void Rotate()
    {
        var direction = (Vector2)(target.position - transform.position).normalized;
        transform.up = direction;
    }

    private bool CanMove() =>
        !Physics2D.Raycast(
              rigidbody.position,
              transform.up,
              stoppingDistance);

}
