using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;


[System.Serializable]
public class HightColoumnData
{
    public int passNumber;
    public bool isTime;
    public HightInfo[] hightInfos;
}

[System.Serializable]
public class HightInfo
{
    public int currentHightNumber;
    public ColumnInfo[] columnInfos;
}
[System.Serializable]
public class ColumnInfo
{
    public int currentColumnNumber;
    public columnCubeType cubeState;
}

public enum columnCubeType
{
    none,
    bomb,
    normal,
}

public static class JsonLoadFile
{
    public static HightColoumnData LoadJsonFormFile(string _path)
    {
        if (!File.Exists(_path))
        {
            return null;
        }

        StreamReader sr = new StreamReader(_path, Encoding.Default);
        if (sr == null)
        {
            return null;
        }

        string jsonText = sr.ReadToEnd();
        if (jsonText.Length > 0)
        {
            return JsonUtility.FromJson<HightColoumnData>(jsonText);
        }

        return null;

    }
}

