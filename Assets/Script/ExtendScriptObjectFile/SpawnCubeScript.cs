using UnityEngine;

[CreateAssetMenu(fileName = "SpawnCube", menuName = "CreateScribtable/DisabledSpawnCubeScript")]
public class SpawnCubeScript : ScriptableObject
{
    public DisabledGeneratingSpawn[] disabledSpawnDatas;

    [System.Serializable]
    public struct DisabledGeneratingSpawn
    {
        [Tooltip("行数")]
        public int heightNumber;
        [Tooltip("列穷举")]
        public int[] conlumnNumbers;
    }

}
