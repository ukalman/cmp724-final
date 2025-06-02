using UnityEngine;

public class HealthModule : ModuleBase
{
    private float _currentHealth;
    private float _maxHealth;
    private float _regenRate;
    private bool _isDead;

    private HealthModuleConfig _config;

    public delegate void HealthChanged(float newValue, float maxValue);
    public event HealthChanged OnHealthChanged;

    public delegate void Died();
    public event Died OnDied;

    public HealthModule(HealthModuleConfig config)
    {
        _config = config;
        _maxHealth = config.maxHealth;
        _regenRate = config.regenRate;
        _currentHealth = _maxHealth;
    }

    public override void Initialize()
    {
        base.Initialize();
        _currentHealth = _maxHealth;
        _isDead = false;
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    public override bool Tick()
    {
        if (!base.Tick() || _isDead) return false;

        if (Input.GetKeyDown(KeyCode.A)) TakeDamage(25);
        
        if (_regenRate > 0.0f && _currentHealth < _maxHealth)
        {
            Heal(_regenRate * Time.deltaTime);
        }

        return true;
    }

    public void TakeDamage(float amount)
    {
        if (_isDead) return;

        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0.0f, _maxHealth);
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

        if (_currentHealth <= 0.0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        if (_isDead) return;

        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0.0f, _maxHealth);
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
    }

    private void Die()
    {
        _isDead = true;
        OnDied?.Invoke();
        Debug.Log("I fucking died!");
    }

    public float GetHealth() => _currentHealth;
    public float GetMaxHealth() => _maxHealth;
    public bool IsDead() => _isDead;
}