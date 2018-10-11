using UnityEngine;
[CreateAssetMenu(fileName = "SpawnCube", menuName = "CreateScribtable/TriggerCubeScriptable")]
public class TriggerCubeScript : ScriptableObject {

    public enum ETriggerState
    {
        None,                   // 无
        Bombx4,                 // 四格爆炸
        Bombx8,                 // 八格爆炸
        BombHorizontal,         // 水平爆炸
        BombVertical,           // 垂直爆炸
        Binding,                // 束缚
        Show,                   // 出现 (假设前方都是无方格，这个预留出现方格)
    }

    [System.Serializable]
    public struct TriggerSendStruct
    {
        public int height;
        [Range(1,4)]
        public float triggerSecond;
        public TriggerExhaustive[] triggerMind;

        [System.Serializable]
        public struct TriggerExhaustive
        {
            public int height, column;
            public ETriggerState state;
        }
    }

    public TriggerSendStruct[] triggerDatas;

}
