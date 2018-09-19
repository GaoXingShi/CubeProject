using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    public enum spawnWay
    {
        random,
        order,
        unOrder,
        center
    }

    public Material[] spawnNormalMaterial;
    private List<GameObject> planeObjArray = new List<GameObject>();
    private int spawnHeight = 0;
    private WaitForSeconds waitforSecond = new WaitForSeconds(0.025f);
    // Use this for initialization
    private IEnumerator Start ()
	{
	    for (int i = 0; i < 16; i++)
	    {
            yield return ISpawnEnumerator(i);
        }
	}

    public void InitSpwanHeightPlaneCube(int _height)
    {
        StartCoroutine(ISpawnEnumerator(_height));
    }

    private IEnumerator ISpawnEnumerator(int _height)
    {
        for (int j = -3; j < 4; j++)
        {
            if (j == -3 && spawnHeight % 2 == 1)
            {
                continue;
            }

            Transform obj = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
            planeObjArray.Add(obj.gameObject);

            obj.position = new Vector3(j * 1.4f - (spawnHeight % 2 * 0.7f), 0, spawnHeight * 0.7f);
            obj.eulerAngles = Vector3.up * 45;
            obj.GetComponent<Renderer>().material = spawnNormalMaterial[_height % spawnNormalMaterial.Length];

            yield return waitforSecond;
        }
        spawnHeight++;

    }

}
