using UnityEngine;

public class CoinFarmScript : MonoBehaviour
{
    [SerializeField] public int cashAmount = 100;
    public bool wasCashCollected;
    private GameObject spawnerObject;


    void Start()
    {
        spawnerObject = GameObject.FindGameObjectWithTag("GameManager");
        wasCashCollected = true;
    }

    void Update()
    {
        if (wasCashCollected == false)
        {
            Money money = spawnerObject.GetComponent<Money>();
            money.IncreaseCash(cashAmount);
            wasCashCollected = true;
        }
    }
}