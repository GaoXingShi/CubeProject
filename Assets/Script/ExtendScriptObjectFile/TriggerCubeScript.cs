
using UnityEngine;

public class TriggerCubeScript : ScriptableObject {

    public enum ETriggerState
    {
        None,
        Bombx4,
        Bombx8,
        BombHorizontal,
        BombVerVertical,
        Binding

    }

    [System.Serializable]
    public struct TriggerSendStruct
    {
        public int height;
        public float triggerSecond;
        public TriggerExhaustive[] triggerMind;

        [System.Serializable]
        public struct TriggerExhaustive
        {
            public int height, column;
            public ETriggerState state;
        }
    }


}
