using System;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

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
    protected Sequence sequence;
    protected Action<bool> ExecuteActon;
    public PlaneCubeScript()
    {
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
        ExecuteActon += RightSizeEffect;
        //ExecuteActon += DownSizeEffect;
        //ExecuteActon += LerpSizeEffect;
        
    }
    

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
    public void Remove()
    {
        state = PlaneCubeState.Creating;
        sequence = DOTween.Sequence();
        ExecuteActon.GetInvocationList()[Random.Range(0, ExecuteActon.GetInvocationList().Length)].DynamicInvoke(false);

    }

    public void RemoveFinish()
    {
        state = PlaneCubeState.Idle;
    }

    public Transform GetCurrentTransform()
    {
        return currTransform;
    }

    public PlaneCubeState GetCurrentState()
    {
        return state;
    }
    

    public int GetHeight()
    {
        return height;
    }

    public int GetWidth()
    {
        return width;
    }

    private void RightSizeEffect(bool _isInit)
    {
        if (_isInit)
        {
            Vector3 InitPos = currTransform.position;
            currTransform.position += Vector3.right * 10;
            sequence.Append(DOTween.To(() => currTransform.position, x => { currTransform.position = x; },
                InitPos, speed * 5));
            sequence.AppendCallback(() => { state = PlaneCubeState.Occupy; });
        }
        else
        {
            Vector3 InitPos = currTransform.position + Vector3.right * 10;
            sequence.Append(DOTween.To(() => currTransform.position, x => { currTransform.position = x; },
                InitPos, speed * 5));
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
    



}
