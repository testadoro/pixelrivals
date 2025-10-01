using UnityEngine;

public class Health : MonoBehaviour
{
    [Min(1)] public int maxHP = 100;
    public int currentHP = 100;

    public System.Action OnDeath;

    private void Awake()
    {
        currentHP = Mathf.Clamp(currentHP, 1, maxHP);
    }

    public void SetMaxHP(int value)
    {
        maxHP = Mathf.Max(1, value);
        currentHP = maxHP;
    }

    public void HealToFull()
    {
        currentHP = maxHP;
    }

    public void Damage(float amount)
    {
        int a = Mathf.CeilToInt(amount);
        currentHP -= a;
        if (currentHP <= 0)
        {
            currentHP = 0;
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}