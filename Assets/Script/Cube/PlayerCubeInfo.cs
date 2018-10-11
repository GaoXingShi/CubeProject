using System.Linq;

public class PlayerCubeInfo
{
    private CubeScript cube;
    private TriggerCubeScript triggerData;
    private int currentTriggerNumber = 0;
    public PlayerCubeInfo(CubeScript _cube)
    {
        cube = _cube;
        triggerData = MainController.Instance.triggerCubeInfo;
        MainController.Instance.playerInfo = this;
    }

    /// <summary>
    /// 设置是否停止
    /// </summary>
    /// <param name="_isPause"></param>
    public void SetCubeState(bool _isPause)
    {
        cube.isPause = _isPause;
    }

    /// <summary>
    /// 用户当前行位置
    /// </summary>
    /// <returns></returns>
    public int GetCubeForwardCount()
    {
        return cube.GetForwardCount();
    }

    //todo 共有方法，检查当前位置是否会导致游戏失败。

    
    /// <summary>
    /// 当前位置是否触发机关，有触发则调用SpawnMes。
    /// </summary>
    /// <returns></returns>
    public void GetGameTrigger(int _fowardNumber)
    {
        foreach (var v in triggerData.triggerDatas.Where(x => x.height == _fowardNumber))
        {
            for (int i = 0; i < v.triggerMind.Length; i++)
            {
                var parameter = v.triggerMind[i];
                SpawnManager.Instance.RemovePlaneCubeAt(parameter.height, parameter.column, v.triggerSecond,
                    parameter.state);
            }
        }
    }
}