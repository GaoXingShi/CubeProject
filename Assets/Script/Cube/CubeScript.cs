using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    public CameraLook camera;

    [HideInInspector]
    public bool isPause;

    private Vector3 bianyuan;

    private float rotaValue;

    private bool isRotate;

    private readonly float root2 = Mathf.Pow(2, 0.5f);

    private Vector3 angleV3 = Vector3.zero;

    private int forwardCount = 0;



    // Update is called once per frame
    void Update()
    {
        if (isPause)
        {
            return;
        }

        if (!isRotate)
        {
            bianyuan = transform.position;
            if (Input.GetKey(KeyCode.A))
            {
                bianyuan.x -= root2 / 4;
                bianyuan.z += root2 / 4;
                bianyuan.y -= bianyuan.y / 2;
                isRotate = true;
                angleV3 = new Vector3(1, 0, 1);
                SpawnManager.Instance.SpawnHeightCountAppend(1);

            }
            else if (Input.GetKey(KeyCode.D))
            {
                bianyuan.x += root2 / 4;
                bianyuan.z += root2 / 4;
                bianyuan.y -= bianyuan.y / 2;
                isRotate = true;
                angleV3 = new Vector3(1, 0, -1);
                SpawnManager.Instance.SpawnHeightCountAppend(1);

            }
        }

        if (isRotate)
        {
            rotaValue += 6;
            transform.RotateAround(bianyuan, angleV3, 6);
            if (rotaValue >= 90)
            {
                rotaValue = 0;
                angleV3 = Vector3.zero;
                isRotate = false;
                forwardCount++;
                camera.GoForward(1);
            }
        }
    }


    public int GetForwardCount()
    {
        return forwardCount;
    }
}
