using UnityEngine;

public class LoadInfoManager : Singleton<LoadInfoManager>
{
    public HightColoumnData hightInfo;
    public string JsonFileName;

    public void LoadJsonFile()
    {
        hightInfo = JsonLoadFile.LoadJsonFormFile(Application.streamingAssetsPath + "/JsonFiles/" + JsonFileName + ".json");
    }

    public HightColoumnData GetHightColumnData()
    {
        return hightInfo;
    }
}
