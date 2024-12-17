using UnityEngine;

public class CoinFarmScript : MonoBehaviour
{
    [SerializeField] public int cashAmount = 100;
    public bool wasCashCollected;
    private GameObject baseObject;
    private GameObject spawnerObject;


    void Start()
    {
        baseObject = GameObject.FindGameObjectWithTag("Base");
        spawnerObject = GameObject.FindGameObjectWithTag("GameManager");
        wasCashCollected = true;
    }

    void Update()
    {
        if (wasCashCollected == false)
        {
            Spawner spawner = spawnerObject.GetComponent<Spawner>();
            Money money = spawnerObject.GetComponent<Money>();
            BaseHealth health = baseObject.GetComponent<BaseHealth>();
            if (health != null && health.healthPositive() && spawner.IsWaveActive())
            {
                money.IncreaseCash(cashAmount);
                wasCashCollected = true;
                Debug.Log("Added 100 cash!");
            }
        }
    }
}