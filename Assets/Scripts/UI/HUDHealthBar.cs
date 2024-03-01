using UnityEngine;
using UnityEngine.UI;

public class HUDHealthBar : MonoBehaviour, IHUDElement
{
    [SerializeField] private Image bar;

    private HP _hp;
    
    public void Init(GameObject character)
    {
        _hp = character.GetComponent<HP>();
        _hp.onDamageReceived += OnDamageReceived;
        _hp.onHpSet += OnHpSet;
        SetFillAmount(_hp.Value, _hp.MaxValue);
    }

    private void OnDamageReceived(HpChangeInfo hpChangeInfo)
    {
        SetFillAmount(hpChangeInfo.newHP, hpChangeInfo.maxHP);
    }

    private void OnHpSet(HpChangeInfo hpChangeInfo)
    {
        SetFillAmount(hpChangeInfo.newHP, hpChangeInfo.maxHP);
    }

    private void SetFillAmount(float hp, float maxHp)
    {
        bar.fillAmount = hp / maxHp;
    }
}
