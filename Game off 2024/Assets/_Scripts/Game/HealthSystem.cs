using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] float _maxHealth;
    [SerializeField] float _glitchDuration;
    [SerializeField] SpriteRenderer _sprite;
    private ColorType _charColorType;
    private bool _isInvulnable = false;


    private float _currentHealth;

    private void Awake()
    {
        GameController.OnPlayerHealthUpdate += UpdateHealth;
        GameController.OnDamagePlayer += TakeDamage;
        GameController.OnChangePlayerColor += ChangeColor;

        _currentHealth = _maxHealth;
        _charColorType = (ColorType)Random.Range(1, 5); //Update later.
    }

    private void OnDestroy()
    {
        GameController.OnPlayerHealthUpdate -= UpdateHealth;
        GameController.OnDamagePlayer -= TakeDamage;
        GameController.OnChangePlayerColor -= ChangeColor;
    }

    void UpdateHealth(float amount)
    {
        if (_isInvulnable) return;
        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0f, _maxHealth);
    }

    void TakeDamage(ColorType colorType, float damage)
    {
        if (_charColorType != colorType && !_isInvulnable)
        {
            UpdateHealth(damage);
            MainController.PlaySound(SoundType.Hurt);
            MainController.UpdateLifeView();
            StartCoroutine(IDamageFlash());
            if (_currentHealth < 1f)
            {
                MainController.EndGame();
            }
        }
    }

    void ChangeColor(ColorType type)
    {
        _charColorType = type;
    }

    private IEnumerator IDamageFlash()
    {
        var charColor = _sprite.color;
        _isInvulnable = true;
        for (int i = 0; i < 2; i++)
        {
            _sprite.color = Color.red;
            yield return new WaitForSeconds(_glitchDuration);
            _sprite.color = charColor;
            yield return new WaitForSeconds(_glitchDuration);
            _sprite.color = Color.red;
            yield return new WaitForSeconds(_glitchDuration);
            _sprite.color = charColor;
            yield return new WaitForSeconds(_glitchDuration);
        }
        _isInvulnable = false;
    }
}
