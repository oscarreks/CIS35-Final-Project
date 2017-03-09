using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBullet : MonoBehaviour {

    public TEAM team;
    public float _speed = 5.0f;
    public float _damage = 10.0f;

	void Update () {
        transform.position += transform.forward * Time.deltaTime * _speed;
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        if (outOfBounds())
        {
            Destroy(this);
        }
        else if (col.gameObject.GetComponent<Unit>().team != team)
        {
            col.gameObject.GetComponent<Unit>().addHealth(_damage);
            Destroy(this);
        }
    }

    public bool outOfBounds()
    {
        return transform.position.x > 10 || 
               transform.position.x < -10 || 
               transform.position.y > 5 || 
               transform.position.y < -5;
    }
}
