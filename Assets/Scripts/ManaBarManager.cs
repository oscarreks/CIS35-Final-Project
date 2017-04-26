using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBarManager : MonoBehaviour
{
    public float maxMana = 10.0f;
    public float maxPixelWidth = 100;
    public float bar_height = 16;
    public TEAM _team = TEAM.RED;

    public Texture2D ManaBarTexture;
    public Texture2D EmptyBarTexture;

    public float x;
    public float y = 20;

    float manaBarLength;
    float manaPercentage;

    void Start()
    {
        if (_team == TEAM.RED)
        {
            x = 32;
        }
        else
        {
            x = Screen.width - 32;
            maxPixelWidth *= -1;
        }



    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(x, y, maxPixelWidth, bar_height), EmptyBarTexture);
        GUI.DrawTexture(new Rect(x, y, manaBarLength, bar_height), ManaBarTexture);
    }

    void Update()
    {
        manaPercentage = GameManager.instance.mana[(int)_team] / maxMana;
        manaBarLength = manaPercentage * maxPixelWidth;
    }
}