using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeScript : MonoBehaviour
{
    public CameraLook camera;

    private Vector3 bianyuan;

    private float rotaValue;

    private bool isRotate;

    private readonly float root2 = Mathf.Pow(2, 0.5f);

    private Vector3 angleV3 = Vector3.zero;
    // Use this for initialization
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (!isRotate)
        {
            bianyuan = transform.position;
            if (Input.GetKeyDown(KeyCode.A))
            {
                bianyuan.x -= root2 / 4;
                bianyuan.z += root2 / 4;
                bianyuan.y -= bianyuan.y / 2;
                isRotate = true;
                angleV3 = new Vector3(1, 0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                bianyuan.x += root2 / 4;
                bianyuan.z += root2 / 4;
                bianyuan.y -= bianyuan.y / 2;
                isRotate = true;
                angleV3 = new Vector3(1, 0, -1);

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
                camera.GoForward(1);
            }
        }
    }
    
}
