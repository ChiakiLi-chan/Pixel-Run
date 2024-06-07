using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PortalScript : MonoBehaviour
{
    public Gamemodes Gamemode;
    public Speeds Speed;

    public PortalValues PortalValue;
    public bool gravity;

    public int State;

    public void initiatePortal(Movement movement)
    {
        movement.ChangeThroughPortal(PortalValue, gravity ? 1 : -1, State, transform.position.y);
    }
}
