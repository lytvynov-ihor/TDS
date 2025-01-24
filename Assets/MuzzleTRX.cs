using UnityEngine;

public class MuzzleTRX : MonoBehaviour
{
    public Transform muzzle;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(this.gameObject, 0.9f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = muzzle.position;
    }
}
