using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Teleporter : MonoBehaviour, IComparable<Teleporter>
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

    public override string ToString()
    {
        return this.name;
    }

    public int CompareTo(Teleporter other)
    {
        if (other == null)
        {
            return 1;
        }
        else
        {
            return this.name.CompareTo(other.name);
        }
    }
}