using UnityEngine;
using PixelRivals.Player;

[RequireComponent(typeof(StatsRuntime))]
[RequireComponent(typeof(Health))]
public class BotController : MonoBehaviour
{
    public Transform target; // impostato a runtime
    public Transform shootOrigin;
    public GameObject projectilePrefab;

    private StatsRuntime _stats;
    private float _attackTimer;

    private void Awake()
    {
        _stats = GetComponent<StatsRuntime>();
    }

    private void Update()
    {
        if (target == null) return;

        Vector3 to = (target.position - transform.position);
        to.y = 0f;
        float dist = to.magnitude;
        if (dist > 0.05f)
        {
            Vector3 dir = to.normalized;
            transform.position += dir * (_stats.MoveSpeed * 0.75f) * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10f * Time.deltaTime);
        }

        _attackTimer -= Time.deltaTime;
        if (dist <= _stats.AttackRange && _attackTimer <= 0f)
        {
            _attackTimer = Mathf.Max(0.4f, _stats.AttackRate * 2f);
            Shoot();
        }
    }

    private void Shoot()
    {
        if (projectilePrefab != null && shootOrigin != null)
        {
            var go = GameObject.Instantiate(projectilePrefab, shootOrigin.position, shootOrigin.rotation);
            var proj = go.GetComponent<Projectile>();
            if (proj != null)
            {
                proj.damage = _stats.AttackDamage * 0.8f;
                proj.range = _stats.AttackRange;
                proj.speed = _stats.ProjectileSpeed;
                proj.owner = this.transform;
            }
            var rb = go.GetComponent<Rigidbody>();
            if (rb != null) rb.velocity = shootOrigin.forward * _stats.ProjectileSpeed;
        }
    }
}