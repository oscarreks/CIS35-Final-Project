using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UNIT_NAME { TANK, MTANK, SOLDIER, BUILDING }
public enum TEAM { RED, GREEN }

public struct Stats
{
    public float attack, health, speed, range, attack_speed;
    public int cost;

    public Stats(float a, float h, float s, int c, float r, float a_s)
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

    public static float sight_radius = 4;

    public static Stats[] index =
    {
        new Stats(10, 45, 1, 4, 2, 1),          //TANK
        new Stats(8, 100, 0.5f, 5, 2, 1.3f),    //MTANK
        new Stats(4, 25, 1.5f, 2, 2, 2),           //SOLDIER
        new Stats(0, 100, 0, 4, 0, 1)           //BUILDING
    };
}
