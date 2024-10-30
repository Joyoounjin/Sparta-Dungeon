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
        curValue = startValue; // 게임 시작 시 초기값 
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

    // fillAmount는 0부터 1 사이의 값으로 채워지는 정도를 나타냄
    // 1이면 다 차있고 0이면 비워져있음
    // 그래서 UI로 표시하기 위해 나누기, EX) 50/100 이면 0.5가 돼서 반만 채워지게 되는 것 
    public float GetPercentage()
    {
        return curValue / maxValue;
    }
}
