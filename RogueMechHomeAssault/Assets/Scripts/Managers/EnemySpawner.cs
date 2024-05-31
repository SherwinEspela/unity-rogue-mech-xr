using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] int totalEnemies = 5;
    [SerializeField] PoolManager poolManager;

    private List<Vector3> spawnPoints;
    private List<Mech> mechs;

    private void Start()
    {
        mechs = new List<Mech>();
    }

    private Vector3 GetRandomSpawnPoint()
    {
        if (spawnPoints.Count == 0)
        {
            Debug.LogError("Spawnpoints should not be empty!");
            return Vector3.zero;
        }

        return spawnPoints[Random.Range(0, spawnPoints.Count - 1)];
    }

    public void SetSpawnPoints(List<Vector3> spawnPoints)
    {
        this.spawnPoints = spawnPoints;
    }

    public void SpawnEnemy()
    {
        if (mechs.Count >= totalEnemies) return;

        var mech = poolManager.SpawnMech();
        mech.OnMechDestinationReached += HandleMechDestinationReached;

        var randomPos = GetRandomSpawnPoint();
        mech.transform.position = randomPos;
        SetNewDestination(mech);

        mechs.Add(mech);

        StartCoroutine(AddEnemyDelayedCoroutine());
    }

    IEnumerator AddEnemyDelayedCoroutine()
    {
        yield return new WaitForSeconds(2.0f);
        SpawnEnemy();
    }

    private void HandleMechDestinationReached(Mech mech)
    {
        SetNewDestination(mech);
    }

    private void SetNewDestination(Mech mech)
    {
        var destination = GetRandomSpawnPoint();
        mech.MoveTo(destination);
    }
}
