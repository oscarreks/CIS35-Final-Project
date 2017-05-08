using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BAR_SIZE {SMALL, MEDIUM, LARGE};

public class HealthBar : MonoBehaviour {

    public BAR_SIZE barSize = BAR_SIZE.MEDIUM;
    private float barScale;

    public float maxHealth;
    public float _current_health;

    public float x_offset;
    public float y_scale = 0.2f;

    float healthPercentage;

    SpriteRenderer _sr;

    void Start()
    {
        //_sr = GetComponent<SpriteRenderer>();

        switch (barSize)
        {
            case BAR_SIZE.SMALL:
                barScale = 0.8f;
                break;
            case BAR_SIZE.MEDIUM:
                barScale = 1f;
                break;
            case BAR_SIZE.LARGE:
                barScale = 1.5f;
                break;
        }
    }

    public void initHealth(float health)
    {
        maxHealth = health;
        _current_health = maxHealth;
    }

    public void addHealth(float health)
    {
        _current_health += health;
    }

    void Update()
    {
        healthPercentage = _current_health / maxHealth;                 // gets a float between 0-1
        //x_offset = (float)maxHealth * (1 - healthPercentage) * 0.5f;    // gets the offset needed to align the bar left

        GetComponent<SpriteRenderer>().transform.localScale = new Vector3(healthPercentage * barScale, y_scale, 1);
    }
}
