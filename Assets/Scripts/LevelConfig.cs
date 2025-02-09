using UnityEngine;

[CreateAssetMenu(fileName = "LevelConfig", menuName = "Game/LevelConfig")]
public class LevelConfig : ScriptableObject
{
    [System.Serializable]
    public class LevelData
    {
        public int level;
        public int scoreLimit;
        public float timeLimit; // Make sure this field exists
    }

    public LevelData[] levels;
}
