using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UNIT_NAME { TANK, SOLDIER }
public enum TEAM { RED, GREEN }

public struct Stats
{
    public float attack, health, speed, cost, range;

    public Stats(float a, float h, float s, float c, float r)
    {
        attack = a;
        health = h;
        speed = s;
        cost = c;
        range = r;
    }
}

public static class UnitStats {

    public static Stats[] index =
    {
        new Stats(10, 45, 3, 5, 2),   //TANK
        new Stats(4, 25, 2, 2, 3)     //SOLDIER
    };
}
