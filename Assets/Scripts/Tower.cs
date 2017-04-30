using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    public float _health;

    void Update()
    {
        if(_health < 0)
        {
            startDeath();
        }
    }

    public void addHealth(float health)
    {
        _health += health;
    }

    public void startDeath()
    {
        Destroy(gameObject);
    }
}
