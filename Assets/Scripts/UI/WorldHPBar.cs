using UnityEngine;
using UnityEngine.UI;

public class WorldHPBar : MonoBehaviour, IDamageReceivedListener, IHpSetListener
{
    [SerializeField] private Image bar;

    private Camera _camera;
    
    private void Start()
    {
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.rotation = _camera.transform.rotation;
    }


    public void OnDamageReceived(HpChangeInfo hpChangeInfo)
    {
        SetFillAmount(hpChangeInfo.newHP, hpChangeInfo.maxHP);
    }


    public void OnHpSet(HpChangeInfo hpChangeInfo)
    {
        SetFillAmount(hpChangeInfo.newHP, hpChangeInfo.maxHP);
    }

    private void SetFillAmount(float hp, float maxHp)
    {
        bar.fillAmount = hp / maxHp;
    }
}
