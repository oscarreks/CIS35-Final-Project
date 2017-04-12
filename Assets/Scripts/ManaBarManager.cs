using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaBarManager : MonoBehaviour
{
    public float curMana = 10f;
    public float maxMana = 10f;
    public float maxPixelWidth = 1000;
    public TEAM _team = TEAM.RED;

    public Texture2D ManaBarTexture;

    public float x = (Screen.width / 2) - 100;
    public float y = 20;

    float manaBarLength;
    float manaPercentage;


    void OnGUI()
    {
        GUI.DrawTexture(new Rect(x, y, manaBarLength, 10), ManaBarTexture);
    }

    void Update()
    {
        //clamp values
        curMana = Mathf.Clamp(curMana, 0, maxMana);

        manaPercentage = GameManager.instance.mana[(int)_team] / maxMana;
        manaBarLength = manaPercentage * maxPixelWidth;

        test_bar();
    }

    void test_bar()
    {
        if (Input.GetKeyDown("m"))
        {
            curMana -= 10.0f;
        }

        if (Input.GetKeyDown("n"))
        {
            curMana += 10;
        }

        if (Input.GetKeyDown("s"))
        {
            transform.position -= new Vector3(0, 2, 0);
        }
    }
}