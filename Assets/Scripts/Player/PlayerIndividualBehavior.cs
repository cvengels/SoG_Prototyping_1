using UnityEngine;

public class PlayerIndividualBehavior : MonoBehaviour
{
    [SerializeField] private CharType playerPrefabType;

    public CharType GetPrefabType()
    {
        return playerPrefabType;
    }
}
