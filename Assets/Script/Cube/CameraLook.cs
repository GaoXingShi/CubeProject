using DG.Tweening;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    private readonly float root2 = Mathf.Pow(2, 0.5f) / 2;
    private float finalZValue;
    private Vector3 temp;
    private void Start()
    {
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
        temp = transform.position;
    }

    public void GoForward(int count)
    {
        temp = temp + Vector3.forward * root2 * count;
        transform.DOMove(temp, 0.75f);
    }

}
