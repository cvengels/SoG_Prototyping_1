using UnityEngine;

public class CheeseSpawnPoint : MonoBehaviour
{
    public bool SpawnCheese()
    {
        if (Instantiate(GameManager.Instance.GetCheesePrefab(), transform.position, Quaternion.identity))
        {
            return true;
        }
        return false;
    }
}
