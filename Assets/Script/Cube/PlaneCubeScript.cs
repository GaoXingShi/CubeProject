using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlaneCubeScript
{
    public enum PlaneCubeState
    {
        Occupy,
        Idle,
        Creating
    }
    

    protected int height, width;
    protected Transform currTransform;
    protected PlaneCubeState state = PlaneCubeState.Idle;
    protected Sequence sequence;

    public PlaneCubeScript()
    {
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
    }

    public void Init(Transform _curr,Material _mater,int _h,int _w,Vector3 _p,Vector3 _e)
    {
        currTransform = _curr;
        height = _h;
        width = _w;
        _curr.position = _p;
        _curr.eulerAngles = _e;
        _curr.localScale = Vector3.one;
        _curr.name = "Cube_" + _h + "_" + _w;

        _curr.GetComponent<Renderer>().material = _mater;
        state = PlaneCubeState.Creating;

        sequence = DOTween.Sequence();
        LerpSizeEffect();
    }
    public void Remove()
    {
        currTransform = null;
        state = PlaneCubeState.Idle;
        height = width = 0;
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

    private void NormalSizeEffect()
    {
        sequence.Append(DOTween.To(() => currTransform.localScale, x => { currTransform.localScale = x; },
            Vector3.one * 2, 0.15f));
        sequence.Join(DOTween.To(() => currTransform.eulerAngles, x => { currTransform.eulerAngles = x; },
            new Vector3(0, 45 + 360, 0), 0.15f));
        sequence.Append(DOTween.To(() => currTransform.localScale, x => { currTransform.localScale = x; },
            Vector3.one, 0.15f));
        sequence.Join(DOTween.To(() => currTransform.eulerAngles, x => { currTransform.eulerAngles = x; },
            new Vector3(0, 45, 0), 0.15f));
        sequence.AppendCallback(() => { state = PlaneCubeState.Occupy; });
        sequence.Play();
    }

    private void DownSizeEffect()
    {
        currTransform.localScale = Vector3.one * 4;
        sequence.Append(DOTween.To(() => currTransform.localScale, x => { currTransform.localScale = x; },
            Vector3.one, 0.3f));
        sequence.AppendCallback(() => { state = PlaneCubeState.Occupy; });
        sequence.Play();
    }

    private void LerpSizeEffect()
    {
        sequence.Append(DOTween.To(() => currTransform.localScale, x => { currTransform.localScale = x; },
            Vector3.one * 0.6f, 0.025f));
        sequence.Append(DOTween.To(() => currTransform.localScale, x => { currTransform.localScale = x; },
            Vector3.one * 1.4f, 0.025f));
        sequence.Append(DOTween.To(() => currTransform.localScale, x => { currTransform.localScale = x; },
            Vector3.one * 0.6f, 0.025f));
        sequence.Append(DOTween.To(() => currTransform.localScale, x => { currTransform.localScale = x; },
            Vector3.one * 1.4f, 0.025f));
        sequence.Append(DOTween.To(() => currTransform.localScale, x => { currTransform.localScale = x; },
            Vector3.one , 0.025f));
        sequence.AppendCallback(() => { state = PlaneCubeState.Occupy; });
        sequence.Play();
    }



}
