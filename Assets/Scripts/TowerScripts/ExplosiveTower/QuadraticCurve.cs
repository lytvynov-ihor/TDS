using System;
using UnityEngine;
using UnityEngine.Jobs;

public class QuadraticCurve : MonoBehaviour
{
    public Transform a;
    public Transform b;
    public Transform control;

    private TowerAttackExplosion attack;

    public Vector3 evaluate(float t)
    {
        Vector3 ac = Vector3.Lerp(a.position, control.position, t);
        Vector3 cb = Vector3.Lerp(control.position, b.position, t);
        return Vector3.Lerp(ac, cb, t);
    }
    private void OnDrawGizmos()
    {
        if (a == null || b == null || control == null)
        {
            return;
        }

        for (int i = 0; i < 20; i++)
        {
            Gizmos.DrawWireSphere(evaluate(i/20f),0.1f);
        }
    }
    
    
}
