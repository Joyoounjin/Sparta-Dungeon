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
        stamina.Add(amount);
    } // ------ 회복 (사용하기 버튼) ------ 

    public void Die()
    {
        Destroy(gameObject);
    } // ------ 죽었을 때 ------

    public void TakePhysicalDamage(int damageAmount)
    {
        HP.Subtract(damageAmount);
        onTakeDamage?.Invoke();
    } // ------ 데미지 입었을 때 ------
}
