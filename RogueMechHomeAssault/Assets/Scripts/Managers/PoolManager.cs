using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] Mech[] mechsToPool;

    private Queue<Mech> mechs;

    private void Awake()
    {
        if (mechsToPool.Length == 0) return;

        mechs = new Queue<Mech>();

        foreach (var mech in mechsToPool)
        {
            mech.gameObject.SetActive(false);
            mechs.Enqueue(mech); 
        }
    }

    public Mech SpawnMech()
    {
        var mech = mechs.Dequeue();
        mech.gameObject.SetActive(true);
        return mech;
    }
}
