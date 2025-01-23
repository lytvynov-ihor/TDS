using TMPro;
using UnityEngine;

public class UIStatsUpdateFactory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalDamageText;
    [SerializeField] private TextMeshProUGUI spawnTimeText;
    [SerializeField] private TextMeshProUGUI upgradeTopText;
    [SerializeField] private TextMeshProUGUI upgradeBotText;

    public void UpdateDamage(int damage)
    {
        totalDamageText.text = damage.ToString();
    }

    public void UpdateSpawnTime(float time)
    {
        spawnTimeText.text = time.ToString()+ "S";
    }

    public void UpdateTopCost(int upgrade)
    {
        upgradeTopText.text = upgrade.ToString();
    }

    public void UpdateBottomCost(int upgrade)
    {
        upgradeBotText.text = upgrade.ToString();
    }
    
    public void TopCostMax()
    {
        upgradeTopText.text = "Max";
    }

    public void BottomCostMax()
    {
        upgradeBotText.text = "Max";
    }
}
