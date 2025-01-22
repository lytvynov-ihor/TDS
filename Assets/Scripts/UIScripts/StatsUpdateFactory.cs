using TMPro;
using UnityEngine;

public class StatsUpdateFactory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalDamageText;
    [SerializeField] private TextMeshProUGUI spawnTimeText;

    public void UpdateDamage(int damage)
    {
        totalDamageText.text = damage.ToString();
    }

    public void UpdateSpawnTime(float time)
    {
        spawnTimeText.text = time.ToString()+ "S";
    }
}
