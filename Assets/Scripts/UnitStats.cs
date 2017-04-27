using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UNIT_NAME { TANK, MTANK, INFANTRY, RECON, BUILDING }
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
        new Stats(15, 100, 0.5f, 5, 2, 1.3f),    //MTANK
        new Stats(4, 25, 1f, 2, 2, 2),           //SOLDIER
        new Stats(8, 30, 1.5f, 3, 2, .75f),     //RECON
        new Stats(0, 100, 0, 4, 0, 1)           //BUILDING
    };
}
