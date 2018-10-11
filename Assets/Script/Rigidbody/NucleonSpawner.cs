using System.Linq;
using UnityEngine;

public class NucleonSpawner : MonoBehaviour
{
    public float timeBetwwenSpawns;
    public float spawnDistance;
    public Nucleon[] nucleonPrefabs;
    private float timeSinceLastSpawn;
    private bool isBegin;
    public void BombMethod()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, spawnDistance);
        foreach (var rb in colliders.Select(coll => coll.GetComponent<Rigidbody>()).Where(rb => rb != null))
        {
            rb.AddExplosionForce(1200, transform.position, spawnDistance);
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            isBegin = !isBegin;
        }
    }

    void FixedUpdate()
    {
        if (!isBegin)
        {
            return;
        }

        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= timeBetwwenSpawns)
        {
            timeSinceLastSpawn -= timeBetwwenSpawns;
            SpawnNucleon();
        }
    }
    void SpawnNucleon()
    {
        Nucleon prefab = nucleonPrefabs[Random.Range(0, nucleonPrefabs.Length)];
        Nucleon spawn = Instantiate<Nucleon>(prefab);
        spawn.transform.localPosition = Random.onUnitSphere * spawnDistance;
    }
}
