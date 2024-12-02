using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseItem : MonoBehaviour
{
    public virtual void onPlayerInteract(PlayerController player)
    {

    }

    public virtual void onPlayerReleased(PlayerController player)
    {

    }

    public virtual void Damage(int amount)
    {

    }

    public virtual void Damage(int amount, PlayerController player)
    {

    }

}
