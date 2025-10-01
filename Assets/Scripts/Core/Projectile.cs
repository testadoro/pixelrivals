using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 20f;
    public float range = 10f;
    public float speed = 18f;
    public Transform owner;

    private Vector3 _startPos;
    private Rigidbody _rb;

    private void Awake()
    {
        _startPos = transform.position;
        _rb = GetComponent<Rigidbody>();
        if (_rb != null) _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
    }

    private void FixedUpdate()
    {
        if (_rb == null)
        {
            transform.position += transform.forward * speed * Time.fixedDeltaTime;
        }
        else
        {
            _rb.velocity = transform.forward * speed;
        }

        if (Vector3.Distance(_startPos, transform.position) >= range)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (owner != null && other.transform.root == owner.root) return;

        var h = other.GetComponentInParent<Health>();
        if (h != null)
        {
            h.Damage(damage);
            Destroy(gameObject);
        }
    }
}