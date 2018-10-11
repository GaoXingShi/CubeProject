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
    private SpawnCubeScript spawnData;
    public void Init ()
    {
        spawnData = MainController.Instance.spawnCubeInfo;
        changeSpeedValue += Camera.main.orthographicSize;

        SpawnHeightCountAppend(16);
        StartCoroutine(IOrderCubeSpawn());
        StartCoroutine(IOrderCubeRemove());

        mindCubeSpwanSpeed = new Thread(ChangePlaneCubeSpeed) {IsBackground = true};
        mindCubeSpwanSpeed.Start();

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
    
    public void RemovePlaneCubeAt(int _height, int _column,float _second, TriggerCubeScript.ETriggerState _cubeState)
    {
        var designation = GetCubeElement(_height, _column);
        if (designation == null)
        {
            CubeDebug.Log(_height, _column, "值为空.", CubeDebug.DebugLogType.warning);
            return;
        }

        if (designation.GetCurrentState() != PlaneCubeScript.PlaneCubeState.Occupy)
        {
            CubeDebug.Log(_height, _column, "未被使用,无法替换.", CubeDebug.DebugLogType.info);
            return;
        }
        
        designation.Replace(MaterialManager.Instance.GetTriggerMaterial(_cubeState), _second, _cubeState);
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

    #region ###Spawn###



    /// <summary>
    /// 检查当前行列是否可以生成。
    /// </summary>
    /// <param name="_spawnHeight"></param>
    /// <param name="_spawnCloumn"></param>
    /// <returns></returns>
    private bool GetHightArray(int _spawnHeight,int _spawnCloumn)
    {
        foreach (var v in spawnData.disabledSpawnDatas.Where(x => x.heightNumber == _spawnHeight))
        {
            if (v.conlumnNumbers.Count(x => x == _spawnCloumn) != 0)
            {
                return true;
            }

        }
        return false;
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

            if (GetHightArray(_height,j))
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

    #endregion

    #region ###Remove###

    
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

    #endregion

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
            int answer = spawnHeight - MainController.Instance.playerInfo.GetCubeForwardCount();
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

    /// <summary>
    /// 设置生成方块的速度
    /// </summary>
    /// <param name="_speed"></param>
    private void SetPlaneCubeSpeed(float _speed)
    {
        PlaneCubeScript.speed = _speed;
    }

    private void OnDestroy()
    {
        mindCubeSpwanSpeed.Abort();
    }
}
