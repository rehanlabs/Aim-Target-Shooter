using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Lifetime")]
    public float lifetime = 3f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Launch(Vector3 direction, float speed)
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        rb.useGravity = false;
        rb.linearVelocity = direction * speed;

        CancelInvoke();
        Invoke(nameof(Deactivate), lifetime);
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision other)
    {
        ITarget target = other.gameObject.GetComponent<ITarget>();
        if (target != null)
        {
            target.OnHit(); // delegate kill/ammo/despawn to target
        }

        Deactivate();
    }
}
