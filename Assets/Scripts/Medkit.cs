﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : Pickup
{
    [SerializeField]
    private float healValue = 50f;

    public override void ActivatePickup()
    {
        Player.Instance.AddHealth(healValue);
    }
}
