using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowAnimationEvents : MonoBehaviour
{
    public PlayerController player;

    public void BowSoot()
    {
        if (player != null)
        {
            player.BowSoot();
        }
    }
}
