using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Singleton<Spawner>
{
    public EZObjectPools.EZObjectPool objectPool;
    public int numberToSpawn;
    public int currNumberOfSpawned;

    /// <summary>
    /// Spawns a single instance of the prefab GameObject.
    /// </summary>
    public void SpawnObject()
    {
        GameObject spawnedObject;
        objectPool.TryGetNextObject(transform.position, transform.rotation, out spawnedObject);
    }

    /// <summary>
    /// Spawns multiple instances of the prefab GameObject.
    /// </summary>
    public void SpawnObject(int _numberToSpawn)
    {
        for(int i = 0; i < _numberToSpawn; ++i)
        {

        }
    }
}
