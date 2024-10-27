using UnityEngine;
using System;

public class PlayerCondition : MonoBehaviour, IDamagable
{
    public UICondition uiCondition;

    Condition HP { get { return uiCondition.HP; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public event Action onTakeDamage;

    private void Update()
    {
        HP.Subtract(HP.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (HP.curValue < 0.0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        HP.Add(amount);
    }

    public void Die()
    {
        Debug.Log("플레이어가 죽었다.");
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        HP.Subtract(damageAmount);
        onTakeDamage?.Invoke();
    }
}
