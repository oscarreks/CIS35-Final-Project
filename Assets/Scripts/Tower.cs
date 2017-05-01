using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    public float _health = 200;

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
        print(gameObject.name + ": " + _health);
    }

    public void startDeath()
    {
        //make cool death animation
        Destroy(gameObject);
    }
}
