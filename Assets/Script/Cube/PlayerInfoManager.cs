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

    //todo 共有方法，检查当前位置是否会导致游戏失败。
	
}
