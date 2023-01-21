using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private SpawnPointType spawnPointType;
    
    public SpawnPointType GetSpawnType()
    {
        return spawnPointType;
    }
}
