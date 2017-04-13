using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

    public float maxHealth;
    public float maxPixelWidth = 32;
    public float pixelHeight = 6;

    public Texture2D HealthBarTexture;

    public float x;
    public float y;
    public float x_offset;
    public float y_offset = -24;
    public Vector2 bar_position;

    Unit parent;
    float healthBarLength;
    float healthPercentage;

    void Start()
    {
        parent = GetComponent<Unit>();
        if(parent == null)
        {
            print("Componenet UNIT not found");
        }
        else
        {
            maxHealth = parent._health;
            x_offset = maxPixelWidth / 2;
        }
    }

    void Update()
    {
        healthPercentage = parent._health / maxHealth;
        healthBarLength = healthPercentage * maxPixelWidth;
        bar_position = Camera.main.WorldToScreenPoint(transform.position);

        x = bar_position.x - x_offset;
        y = Camera.main.pixelHeight - bar_position.y + y_offset;
        
    }

    void OnGUI()
    {
        GUI.DrawTexture(new Rect(x, y, healthBarLength, pixelHeight), HealthBarTexture);
    }
}
