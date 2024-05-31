using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private Queue<Mech> mechsPool;

    public void Initialize(List<Mech> poolingMech)
    {
        mechsPool = new Queue<Mech>();

        foreach (var mech in poolingMech)
        {
            mech.gameObject.SetActive(false);
            mech.transform.position = Vector3.zero;
            mechsPool.Enqueue(mech);
        }
    }

    public Mech SpawnMech()
    {
        var mech = mechsPool.Dequeue();
        mech.gameObject.SetActive(true);
        return mech;
    }
}
