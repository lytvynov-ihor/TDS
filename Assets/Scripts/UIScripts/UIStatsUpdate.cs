using TMPro;
using UnityEngine;

public class UIStatsUpdate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI atkSpeedText;
    [SerializeField] private TextMeshProUGUI rangeText;

    public void UpdateDamageText(int damage)
    {
        damageText.text = damage.ToString();
    }
    
    public void UpdateAtkSpeedText(float speed)
    {
        atkSpeedText.text = speed.ToString()+" s";
    }
    
    public void UpdateRangeText(float range)
    {
        rangeText.text = range.ToString();
    }
}
