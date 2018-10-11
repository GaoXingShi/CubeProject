using System;
using System.Collections;
using System.Threading;
using System.Timers;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;
using Timer = System.Timers.Timer;

public class PlaneCubeScript
{
    public enum PlaneCubeState
    {
        Occupy,
        Idle,
        Creating,
        RemoveOver,
    }
    public static float speed = 0.2f;
    
    protected int height, width;
    protected Transform currTransform;
    protected PlaneCubeState state = PlaneCubeState.Idle;
    private TriggerCubeScript.ETriggerState nowTriggerState = TriggerCubeScript.ETriggerState.None;
    protected Sequence sequence;
    protected Action<bool> ExecuteActon;
    private Timer replaceTimerComponent;

    public PlaneCubeScript()
    {
        replaceTimerComponent = new Timer();
        replaceTimerComponent.Elapsed += ReplaceAfter;
        replaceTimerComponent.AutoReset = false;

        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
        ExecuteActon += NormalEffect;
        //ExecuteActon += DownSizeEffect;
        //ExecuteActon += LerpSizeEffect;
        
    }
    
    /// <summary>
    /// 方块初始化
    /// </summary>
    /// <param name="_curr"></param>
    /// <param name="_mater"></param>
    /// <param name="_h"></param>
    /// <param name="_w"></param>
    /// <param name="_p"></param>
    /// <param name="_e"></param>
    public void Init(Transform _curr,Material _mater,int _h,int _w,Vector3 _p,Vector3 _e)
    {
        currTransform = _curr;
        height = _h;
        width = _w;
        _curr.gameObject.SetActive(true);
        _curr.position = _p;
        _curr.eulerAngles = _e;
        _curr.localScale = Vector3.one;
        _curr.name = "Cube_" + _h + "_" + _w;

        _curr.GetComponent<Renderer>().material = _mater;
        state = PlaneCubeState.Creating;

        sequence = DOTween.Sequence();
        //ExecuteActon.GetInvocationList()[0].DynamicInvoke();
        ExecuteActon.GetInvocationList()[Random.Range(0, ExecuteActon.GetInvocationList().Length)].DynamicInvoke(true);
    }

    /// <summary>
    /// 替换
    /// </summary>
    /// <param name="_mater">替换的材质</param>
    /// <param name="_triggerSecond">触发的秒数</param>
    /// <param name="_cubeState">触发的类型</param>
    public void Replace(Material _mater,float _triggerSecond,TriggerCubeScript.ETriggerState _cubeState)
    {
        if (!currTransform)
        {
            return;
        }

        currTransform.GetComponent<Renderer>().material = _mater;
        currTransform.GetComponent<Renderer>().material.DOColor(Color.red, _triggerSecond);

        nowTriggerState = _cubeState;
        replaceTimerComponent.Interval = _triggerSecond * 1000;
        replaceTimerComponent.Start();
    }

    /// <summary>
    /// 方块初始化的消除,到时的末尾消除。
    /// </summary>
    public void Remove()
    {
        state = PlaneCubeState.Creating;
        sequence = DOTween.Sequence();
        ExecuteActon.GetInvocationList()[Random.Range(0, ExecuteActon.GetInvocationList().Length)].DynamicInvoke(false);

        // 计数器如果被消除，则不能触发。
        replaceTimerComponent.Stop();
    }
    
    [Obsolete("暂时不使用的方法")]
    public void RemoveFinish()
    {
        state = PlaneCubeState.Idle;
    }

    /// <summary>
    /// 获取该脚本控制物体
    /// </summary>
    /// <returns></returns>
    public Transform GetCurrentTransform()
    {
        return currTransform;
    }
    /// <summary>
    /// 获取物体当前的回收状况
    /// </summary>
    /// <returns></returns>
    public PlaneCubeState GetCurrentState()
    {
        return state;
    }
    
    /// <summary>
    /// 获取物体的行
    /// </summary>
    /// <returns></returns>
    public int GetHeight()
    {
        return height;
    }
    /// <summary>
    /// 获取物体的列
    /// </summary>
    /// <returns></returns>
    public int GetWidth()
    {
        return width;
    }

    #region ###SpecialRemove###

    /// <summary>
    /// 替换选择
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="elapsedEventArgs"></param>
    private void ReplaceAfter(object sender, ElapsedEventArgs elapsedEventArgs)
    {
        Dispatcher.InvokeAsync(SelectRemove);
    }

    /// <summary>
    /// 特殊消除，选择阶段
    /// </summary>
    private void SelectRemove()
    {
        switch (nowTriggerState)
        {
            case TriggerCubeScript.ETriggerState.None:

                BombRemove();

                break;
            case TriggerCubeScript.ETriggerState.Bombx4:
                break;
            case TriggerCubeScript.ETriggerState.Bombx8:
                break;
            case TriggerCubeScript.ETriggerState.BombHorizontal:
                break;
            case TriggerCubeScript.ETriggerState.BombVertical:
                break;
            case TriggerCubeScript.ETriggerState.Binding:
                break;
        }
    }

    /// <summary>
    /// 单个消除
    /// </summary>
    private void BombRemove()
    {
        if (state != PlaneCubeState.Occupy)
        {
            CubeDebug.Log(height, width, "无法覆盖", CubeDebug.DebugLogType.info);
            return;
        }

        state = PlaneCubeState.Creating;
        sequence = DOTween.Sequence();
        ExecuteActon.GetInvocationList()[Random.Range(0, ExecuteActon.GetInvocationList().Length)].DynamicInvoke(false);
    }

    /// <summary>
    /// 多个消除
    /// </summary>
    /// <param name="_count"></param>
    private void BombRemove(int _count)
    {

    }

    /// <summary>
    /// 整行或整列消除
    /// </summary>
    /// <param name="_isHorizontal"></param>
    private void BombRemove(bool _isHorizontal)
    {

    }

    #endregion

    #region ###Effect###


    private void NormalEffect(bool _isInit)
    {
        //todo 参数加一个速度。 并取消speed。
        if (_isInit)
        {
            Vector3 initPos = currTransform.position;
            currTransform.position += Vector3.right * 10;
            sequence.Append(DOTween.To(() => currTransform.position, x => { currTransform.position = x; },
                initPos, speed * 5));
            sequence.AppendCallback(() => { state = PlaneCubeState.Occupy; });
        }
        else
        {
            sequence.Append(DOTween.To(() => currTransform.localScale, x => { currTransform.localScale = x; },
                Vector3.zero, speed * 5));
            sequence.Join(DOTween.To(() => currTransform.localEulerAngles, x => { currTransform.localEulerAngles = x; },
                currTransform.localEulerAngles + currTransform.right * 360, speed * 5));
            sequence.AppendCallback(() =>
            {
                state = PlaneCubeState.Idle;
                height = width = 0;
                currTransform.gameObject.SetActive(false);
            });

        }

        sequence.Play();
    }

    private void DownSizeEffect(bool _isInit)
    {
        if (_isInit)
        {
            currTransform.localScale = Vector3.one * 4;
            sequence.Append(DOTween.To(() => currTransform.localScale, x => { currTransform.localScale = x; },
                Vector3.one, speed * 5));
            sequence.AppendCallback(() => { state = PlaneCubeState.Occupy; });
        }

        sequence.Play();
    }

    private void LerpSizeEffect(bool _isInit)
    {
        if (_isInit)
        {
            sequence.Append(DOTween.To(() => currTransform.localScale, x => { currTransform.localScale = x; },
                Vector3.one * 0.6f, speed));
            sequence.Append(DOTween.To(() => currTransform.localScale, x => { currTransform.localScale = x; },
                Vector3.one * 1.4f, speed));
            sequence.Append(DOTween.To(() => currTransform.localScale, x => { currTransform.localScale = x; },
                Vector3.one * 0.6f, speed));
            sequence.Append(DOTween.To(() => currTransform.localScale, x => { currTransform.localScale = x; },
                Vector3.one * 1.4f, speed));
            sequence.Append(DOTween.To(() => currTransform.localScale, x => { currTransform.localScale = x; },
                Vector3.one, speed));
            sequence.AppendCallback(() => { state = PlaneCubeState.Occupy; });
        }

        sequence.Play();
    }
    
    
    #endregion

    ~PlaneCubeScript()
    {
        replaceTimerComponent.Dispose();
    }

}
