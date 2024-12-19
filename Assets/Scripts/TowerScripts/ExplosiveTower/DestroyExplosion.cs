using Unity.VisualScripting;
using UnityEngine;

public class DestroyExplosion : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(this.GameObject(), 2f);
    }

}
