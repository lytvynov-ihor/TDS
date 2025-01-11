using UnityEngine;

public class EnemyBoss : MonoBehaviour
{
    public AudioClip bossTheme;

    void Start()
    {
        if (bossTheme != null)
        {
            AudioManager.Instance.PlayClip(bossTheme);
        }
        else
        {
            Debug.LogWarning("BossTheme is missing for " + gameObject.name);
        }
    }
}