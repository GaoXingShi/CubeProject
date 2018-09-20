using System;
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
    public Transform cubeParent;
    private List<PlaneCubeScript> planeObjArray = new List<PlaneCubeScript>();
    private int spawnHeight = 0;
    private WaitForSeconds waitforSecond = new WaitForSeconds(0.025f);
    private readonly float root2 = Mathf.Pow(2, 0.5f);
    private const int COLUMN = 7;
    private void Start ()
    {
        InitCurrentHeightCount(20);
	}

    public void InitCurrentHeightCount(int _count)
    {
        StartCoroutine(IOrderSpawn(_count));
    }

    public PlaneCubeScript GetCubeElement(int _height, int _column)
    {
        PlaneCubeScript cube = null;
        List<PlaneCubeScript> temp =(from v in planeObjArray where v.GetHeight() == _height && v.GetWidth() == _column select v).ToList();
        if (temp.Count == 1)
        {
            cube = temp.Last();
        }

        return cube;
    }

    private IEnumerator IOrderSpawn(int _count)
    {
        for (int i = 0; i < _count; i++)
        {
            yield return ISpawnEnumerator(i);
        }
    }

    private IEnumerator ISpawnEnumerator(int _height)
    {
        List<PlaneCubeScript> localTempList = new List<PlaneCubeScript>();
        for (int j = 0; j < COLUMN; j++)
        {
            if (j == 0 && spawnHeight % 2 == 1)
            {
                continue;
            }

            Transform obj = null;
            PlaneCubeScript temp = null;
            foreach (var v in planeObjArray)
            {
                if (v.GetCurrentState() == PlaneCubeScript.PlaneCubeState.Idle)
                {
                    temp = v;
                }
            }

            if (temp == null)
            {
                obj = GameObject.CreatePrimitive(PrimitiveType.Cube).transform;
                obj.SetParent(cubeParent.transform);
                temp = new PlaneCubeScript();
                planeObjArray.Add(temp);
            }
            else
            {
                obj = temp.GetCurrentTransform();
            }

            localTempList.Add(temp);

            Vector3 pos = new Vector3(j * root2 - (spawnHeight % 2 * root2 /2), 0, spawnHeight * root2 /2);
            Vector3 eur = Vector3.up * 45;
            temp.Init(obj, spawnNormalMaterial[_height % spawnNormalMaterial.Length], _height, j, pos, eur);
            
            yield return waitforSecond;
        }
        spawnHeight++;

        while (true)
        {
            int count = 0;
            foreach (var v in localTempList)
            {
                if (v.GetCurrentState() == PlaneCubeScript.PlaneCubeState.Occupy)
                {
                    count++;
                }
            }

            if (count == localTempList.Count)
            {
                break;
            }

            yield return null;
        }
    }
    

}
