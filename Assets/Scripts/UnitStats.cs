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
        new Stats(30, 120, 1.2f, 4, 2.5f, 1f),          //TANK
        new Stats(50, 200, 0.8f, 5, 2.5f, 1.2f),   //MTANK
        new Stats(15, 80, 1.2f, 2, 2, 0.75f),       //INFANTRY
        new Stats(20, 100, 1.6f, 3, 2, 0.5f),     //RECON
        new Stats(0, 100, 0, 4, 0, 1)           //BUILDING
    };
}
