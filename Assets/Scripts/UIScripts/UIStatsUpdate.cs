using TMPro;
using UnityEngine;

public class UIStatsUpdate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI atkSpeedText;
    [SerializeField] private TextMeshProUGUI rangeText;
    [SerializeField] private TextMeshProUGUI topCostText;
    [SerializeField] private TextMeshProUGUI bottomCostText;

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

    public void UpdateTopCost(int cost)
    {
        topCostText.text = cost.ToString()+"$";
    }
    
    public void UpdateBottomCost(int cost)
    {
        bottomCostText.text = cost.ToString()+"$";
    }

    public void TopCostMax()
    {
        topCostText.text = "Max";
    }

    public void BottomCostMax()
    {
        bottomCostText.text = "Max";
    }
}
