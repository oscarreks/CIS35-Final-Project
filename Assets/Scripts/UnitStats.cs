using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UNIT_NAME { TANK, MTANK, SOLDIER }
public enum TEAM { RED, GREEN }

public struct Stats
{
    public float attack, health, speed, cost, range, attack_speed;

    public Stats(float a, float h, float s, float c, float r, float a_s)
    {
        attack = a;
        health = h;
        speed = s;
        cost = c;
        range = r;
        attack_speed = a_s;
    }
}

public static class UnitStats {

    public static Stats[] index =
    {
        new Stats(10, 45, 1, 5, 2, 1),      //TANK
        new Stats(10, 65, 0.5f, 7, 2, 1.5f),   //MTANK
        new Stats(4, 25, 2, 2, 3, 2)        //SOLDIER
    };
}
