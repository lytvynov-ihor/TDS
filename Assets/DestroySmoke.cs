using Unity.VisualScripting;
using UnityEngine;

public class DestroySmoke : MonoBehaviour
{
    void Start()
    {
        Destroy(this.GameObject(), 0.5f);
    }

}
