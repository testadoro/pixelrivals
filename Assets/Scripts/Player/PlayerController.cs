using UnityEngine;
using PixelRivals.Player;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(StatsRuntime))]
[RequireComponent(typeof(Health))]
public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    public PixelRivals.Inputs.VirtualJoystick joystick; // collegare da Canvas
    public Transform shootOrigin;
    public GameObject projectilePrefab;

    private CharacterController _cc;
    private StatsRuntime _stats;

    private float _attackTimer;
    private float _ultimateTimer;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _stats = GetComponent<StatsRuntime>();
    }

    private void Update()
    {
        // Movimento
        Vector2 move = joystick != null ? joystick.Direction : Vector2.zero;
        Vector3 dir = new Vector3(move.x, 0f, move.y);
        if (dir.sqrMagnitude > 1f) dir.Normalize();
        _cc.SimpleMove(dir * _stats.MoveSpeed);

        // Timers
        _attackTimer -= Time.deltaTime;
        _ultimateTimer -= Time.deltaTime;

        // Orienta verso direzione di movimento se c'è input
        if (dir.sqrMagnitude > 0.001f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 12f * Time.deltaTime);
    }

    public void Attack()
    {
        if (_attackTimer > 0f) return;
        _attackTimer = _stats.AttackRate;

        if (projectilePrefab != null && shootOrigin != null)
        {
            var go = GameObject.Instantiate(projectilePrefab, shootOrigin.position, shootOrigin.rotation);
            var proj = go.GetComponent<Projectile>();
            if (proj != null)
            {
                proj.damage = _stats.AttackDamage;
                proj.range = _stats.AttackRange;
                proj.speed = _stats.ProjectileSpeed;
                proj.owner = this.transform;
            }
            var rb = go.GetComponent<Rigidbody>();
            if (rb != null) rb.velocity = shootOrigin.forward * _stats.ProjectileSpeed;
        }
    }

    public void Ultimate()
    {
        if (_ultimateTimer > 0f) return;
        _ultimateTimer = _stats.UltimateCooldown;

        var hits = Physics.OverlapSphere(transform.position, _stats.UltimateRadius);
        foreach (var col in hits)
        {
            if (col.attachedRigidbody == null) continue;
            if (col.transform.root == transform.root) continue;
            var health = col.GetComponentInParent<Health>();
            if (health != null)
            {
                health.Damage(_stats.UltimateDamage);
            }
        }
        // TODO: VFX/SFX
    }
}