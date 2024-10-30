using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Condition : MonoBehaviour
{
    public float curValue;
    public float maxValue;
    public float startValue;
    public float passiveValue;
    public Image uiBar;

    private void Start()
    {
        curValue = startValue; // ���� ���� �� �ʱⰪ 
    }

    private void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    public void Add(float amount)
    {
        curValue = Mathf.Min(curValue + amount, maxValue);
    }

    public void Subtract(float amount)
    {
        curValue = Mathf.Max(curValue - amount, 0.0f);
    }

    // fillAmount�� 0���� 1 ������ ������ ä������ ������ ��Ÿ��
    // 1�̸� �� ���ְ� 0�̸� ���������
    // �׷��� UI�� ǥ���ϱ� ���� ������, EX) 50/100 �̸� 0.5�� �ż� �ݸ� ä������ �Ǵ� �� 
    public float GetPercentage()
    {
        return curValue / maxValue;
    }
}
