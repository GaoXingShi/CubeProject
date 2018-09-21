using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    // 外部数据
    public Material[] spawnNormalMaterial;
    public Transform cubeParent;

    // 核心数据
    private List<PlaneCubeScript> planeObjArray = new List<PlaneCubeScript>();
    private int spawnHeight = 0;
    private int spawnHeightRemain = 0;
    private int removeHeight = 0;
    private float planeCubeSpeed = 0.025f;

    // 常数属性
    private readonly WaitForSeconds WAITFOR_SECOND = new WaitForSeconds(0.025f);
    private readonly float ROOT2 = Mathf.Pow(2, 0.5f);
    private const int COLUMN = 7;

    private void Start ()
    {
        SpawnHeightCountAppend(12);
        StartCoroutine(IOrderCubeSpawn());
    }

    public void ChangePlaneCubeSpeed(int _currCubeNumber)
    {
        int answer = spawnHeight - _currCubeNumber;

        if (answer > COLUMN)
        {
            planeCubeSpeed = 0.025f;
        }
        else if (answer > COLUMN / 2)
        {
            planeCubeSpeed = 0.01f;
        }
        else
        {
            planeCubeSpeed = 0;
        }
        
        ChangeSpeed();
    }

    /// <summary>
    /// 在生成队列上追加_count行数。
    /// </summary>
    /// <param name="_count">行数</param>
    public void SpawnHeightCountAppend(int _count)
    {
        spawnHeightRemain += _count;
    }

    /// <summary>
    /// 输入行与列，获取相应脚本。   注意：第1行的下标为0，并且偶数行没有第0个数。
    /// </summary>
    /// <param name="_height"></param>
    /// <param name="_column"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 输入行，获取相应List列
    /// </summary>
    /// <param name="_height"></param>
    /// <returns></returns>
    public List<PlaneCubeScript> GetCubeHeightElement(int _height)
    {
        List<PlaneCubeScript> temp = (from v in planeObjArray where v.GetHeight() == _height select v).ToList();
        return temp;
    }

    /// <summary>
    /// 改变速度
    /// </summary>
    private void ChangeSpeed()
    {
        foreach (var v in planeObjArray)
        {
            v.ChangeSpeedValue(planeCubeSpeed);
        }
    }

    /// <summary>
    /// 循环生成，以顺序表的方式。
    /// </summary>
    /// <returns></returns>
    private IEnumerator IOrderCubeSpawn()
    {
        while (true)
        {
            if (spawnHeightRemain == 0)
            {
                yield return WAITFOR_SECOND;
                continue;
            }

            List<PlaneCubeScript> localTempList = new List<PlaneCubeScript>();
            yield return IColumnSpawn(localTempList, spawnHeight);
            yield return ICheckSpawn(localTempList);
            spawnHeight++;
            spawnHeightRemain--;
        }
    }

    private IEnumerator IOrderCubeRemove()
    {
        while (true)
        {
            if (removeHeight == spawnHeight)
            {
                break;
            }

            List<PlaneCubeScript> localTempList = new List<PlaneCubeScript>();
            localTempList = GetCubeHeightElement(removeHeight);
            yield return ICheckRemove(localTempList);
            yield return IColumnRemove(localTempList);
            removeHeight++;
        }
    }
    /// <summary>
    /// 实施回收
    /// </summary>
    /// <param name="_localTempList"></param>
    /// <returns></returns>
    private IEnumerator ICheckRemove(List<PlaneCubeScript> _localTempList)
    {
        foreach (var v in _localTempList)
        {
            v.Remove();
            yield return WAITFOR_SECOND;
        }
    }
    /// <summary>
    /// 检测回收
    /// </summary>
    /// <param name="_localTempList"></param>
    /// <returns></returns>
    private IEnumerator IColumnRemove(List<PlaneCubeScript> _localTempList)
    {
        while (true)
        {
            int count = 0;
            foreach (var v in _localTempList)
            {
                if (v.GetCurrentState() == PlaneCubeScript.PlaneCubeState.Idle)
                {
                    count++;
                }
            }

            if (count == _localTempList.Count)
            {
                break;
            }

            yield return null;
        }
    }

    
    /// <summary>
    /// 检查生成
    /// </summary>
    /// <param name="_localTempList"></param>
    /// <returns></returns>
    private IEnumerator ICheckSpawn(List<PlaneCubeScript> _localTempList)
    {
        while (true)
        {
            int count = 0;
            foreach (var v in _localTempList)
            {
                if (v.GetCurrentState() == PlaneCubeScript.PlaneCubeState.Occupy)
                {
                    count++;
                }
            }

            if (count == _localTempList.Count)
            {
                break;
            }

            yield return null;
        }
    }
    /// <summary>
    /// 按行数生成列
    /// </summary>
    /// <param name="_localTempList"></param>
    /// <param name="_height"></param>
    /// <returns></returns>
    private IEnumerator IColumnSpawn(List<PlaneCubeScript> _localTempList, int _height)
    {
        for (int j = 0; j < COLUMN; j++)
        {
            if (j == 0 && _height % 2 == 1)
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

            _localTempList.Add(temp);

            Vector3 pos = new Vector3(j * ROOT2 - (_height % 2 * ROOT2 / 2), 0, _height * ROOT2 / 2);
            Vector3 eur = Vector3.up * 45;
            temp.Init(obj, spawnNormalMaterial[spawnHeight % spawnNormalMaterial.Length], spawnHeight, j, pos, eur);

            yield return WAITFOR_SECOND;
        }
    }
}
