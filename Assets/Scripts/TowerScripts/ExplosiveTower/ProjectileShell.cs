using System;
using Unity.VisualScripting;
using UnityEngine;

public class ProjectileShell : MonoBehaviour
{
    [SerializeField] AudioClip explosionAudioClip;
    GameObject camera;

    public QuadraticCurve curve;
    public int attackDamage;
    public float speed;
    public ParticleSystem particle;

    private float sampleTime;

    void Start()
    {
        camera = GameObject.Find("Main Camera");
        sampleTime = 0f;
    }

    void Update()
    {
        sampleTime += Time.deltaTime * speed;
        transform.position = curve.evaluate(sampleTime);
        transform.forward = curve.evaluate(sampleTime+0.001f) - transform.position;
        transform.Rotate(transform.rotation.x,transform.rotation.y,sampleTime*145);

        if (sampleTime >= 0.5f)
        {
            speed += 0.03f;
        }

        if (sampleTime >= 1f)
        {
            Instantiate(particle, transform.position, Quaternion.identity);
            Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);
            foreach (Collider c in colliders)
            {
                if (c.CompareTag("Enemy"))
                {
                    if(c.gameObject.GetComponent<Immunities>().explosionProof)
                        continue;
                
                    EnemyHealth health = c.GetComponent<EnemyHealth>();
                    if (health != null)
                    {
                        health.TakeDamage(attackDamage);
                    }
                }
            }
            
            AudioSource.PlayClipAtPoint(explosionAudioClip, camera.transform.position, 0.4f);
            Destroy(curve.GameObject());
            Destroy(curve.b.GameObject());
            Destroy(curve.control.GameObject());
            Destroy(gameObject);
        }
    }
}
