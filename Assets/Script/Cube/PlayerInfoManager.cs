using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoManager : Singleton<PlayerInfoManager>
{

    public CubeScript cube;

    public void NoticeSpawnManager(int _forwardCount)
    {
        SpawnManager.Instance.ChangePlaneCubeSpeed(_forwardCount);
    }
	
}
