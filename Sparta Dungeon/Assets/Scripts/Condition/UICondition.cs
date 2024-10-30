using UnityEngine;

public class UICondition : MonoBehaviour
{
    public Condition HP;
    public Condition stamina;

    private void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}
