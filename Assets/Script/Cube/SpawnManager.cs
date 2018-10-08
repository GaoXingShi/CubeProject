using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    // 外部数据
    public Material[] spawnNormalMaterial;
    public Transform cubeParent;
    public Transform planeCube;
    public bool isOpenThread = false;
    // 核心数据
    private List<PlaneCubeScript> planeObjArray = new List<PlaneCubeScript>();
    private int spawnHeight = 0;
    private int spawnHeightRemain = 0;
    private int removeHeight = 0;

    // 常数属性
    private readonly WaitForSeconds WAITFOR_SECOND = new WaitForSeconds(0.005f);
    private WaitForSeconds removingWaitSecondValue = new WaitForSeconds(1);
    private readonly float ROOT2 = Mathf.Pow(2, 0.5f);
    private const int COLUMN = 9;
    private float changeSpeedValue = 3;
    private Thread mindCubeSpwanSpeed;
    private HightColoumnData spawnData;
    public void Init ()
    {

        spawnData = LoadInfoManager.Instance.GetHightColumnData();
        changeSpeedValue += Camera.main.orthographicSize;

        SpawnHeightCountAppend(16);
        StartCoroutine(IOrderCubeSpawn());
        StartCoroutine(IOrderCubeRemove());

        mindCubeSpwanSpeed = new Thread(ChangePlaneCubeSpeed) {IsBackground = true};
        mindCubeSpwanSpeed.Start();

    }

    /// <summary>
    /// 设置生成方块的速度
    /// </summary>
    /// <param name="_speed"></param>
    public void SetPlaneCubeSpeed(float _speed)
    {
        PlaneCubeScript.speed = _speed;
    }
    /// <summary>
    /// 设置移除方块的速度
    /// </summary>
    /// <param name="_speed"></param>
    public void SetPlaneCubeRemovingSpeed(float _speed)
    {
        removingWaitSecondValue = new WaitForSeconds(_speed);
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
    private List<PlaneCubeScript> GetCubeHeightElement(int _height)
    {
        List<PlaneCubeScript> temp = (from v in planeObjArray where v.GetHeight() == _height select v).ToList();
        return temp;
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
            //yield return ICheckSpawn(localTempList);
            spawnHeight++;
            spawnHeightRemain--;
        }
    }

    private HightInfo GetHightArray(int _spawnHeight)
    {
        if (_spawnHeight >= spawnData.hightInfos.Length)
        {
            return null;
        }

        return spawnData.hightInfos[_spawnHeight];
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
        HightInfo hightInfo = GetHightArray(_height);
        if (hightInfo != null)
        {
            //todo
             
        }
        for (int j = 0; j < COLUMN; j++)
        {
            if (j == 0 && _height % 2 == 1)
            {
                continue;
            }

            bool isNull = false;
            columnCubeType spawnCubeState = columnCubeType.normal;
            if (hightInfo != null)
            {
                foreach (var v in hightInfo.columnInfos.Where(v => v.currentColumnNumber == j))
                {
                    Debug.Log(v.currentColumnNumber);
                    if (v.cubeState == columnCubeType.none)
                    {
                        isNull = true;
                    }
                    else
                    {
                        spawnCubeState = v.cubeState;
                        isNull = false;
                    }

                    break;
                }
            }

            if (isNull)
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
                obj = Instantiate(planeCube);
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


    /// <summary>
    /// 循环删除。
    /// </summary>
    /// <returns></returns>
    private IEnumerator IOrderCubeRemove()
    {
        yield return new WaitForSeconds(5);
        while (true)
        {
            if (removeHeight == spawnHeight)
            {
                yield return WAITFOR_SECOND;
                continue;
            }

            List<PlaneCubeScript> localTempList = new List<PlaneCubeScript>();
            localTempList = GetCubeHeightElement(removeHeight);
            yield return ICheckRemove(localTempList);
            yield return removingWaitSecondValue;
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
                if (v.GetCurrentState() == PlaneCubeScript.PlaneCubeState.RemoveOver)
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

        foreach (var v in _localTempList)
        {
            v.RemoveFinish();
        }
    }


    /// <summary>
    /// 多线程，检测玩家位置，实施更新方块生成速度
    /// </summary>
    private void ChangePlaneCubeSpeed()
    {
        while (true)
        {
            if (!isOpenThread)
            {
                Thread.Sleep(500);
                continue;
            }
            int answer = spawnHeight - PlayerInfoManager.Instance.GetCubeForwardCount();
            float planeCubeSpeed;
            if (answer > changeSpeedValue)
            {
                planeCubeSpeed = 0.2f;
            }
            else if (answer > changeSpeedValue / 2)
            {
                planeCubeSpeed = 0.05f;
            }
            else
            {
                planeCubeSpeed = 0.002f;
            }

            SetPlaneCubeSpeed(planeCubeSpeed);
            Thread.Sleep(500);
        }
    }

    private void OnDestroy()
    {
        mindCubeSpwanSpeed.Abort();
    }
}
