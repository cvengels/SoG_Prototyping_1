using UnityEngine;

public class Teleporter : MonoBehaviour
{
    
    [SerializeField] private bool isMouseHole;
    [SerializeField] private RoomsLevel1 destination;

    public bool IsMouseHole()
    {
        return isMouseHole;
    }

    public RoomsLevel1 GetTeleporterDestination()
    {
        return destination;
    }
    
}