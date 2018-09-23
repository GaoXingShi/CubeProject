using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoManager : Singleton<PlayerInfoManager>
{

    public CubeScript cube;

    public void SetCubeState(bool _isPause)
    {
        cube.isPause = _isPause;
    }

    public int GetCubeForwardCount()
    {
        return cube.GetForwardCount();
    }
	
}
