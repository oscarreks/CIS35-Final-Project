using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBullet : MonoBehaviour {

    public TEAM _team;
    public float _speed = 24.0f;
    public float _damage = 10.0f;

    private Rigidbody2D _rb;
    private ConstantForce2D _cf;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _cf = GetComponent<ConstantForce2D>();

        _rb.AddRelativeForce(Vector2.right * _speed);
        _cf.relativeForce = new Vector2(_speed, 0);
    }

	void FixedUpdate () {
        if (outOfBounds())
        {
            Destroy(gameObject);
        }
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.GetComponent<Unit>() && !other.gameObject.GetComponent<Tower>())
        {
            return;
        }

        if (other.gameObject.GetComponent<Unit>()._team != _team)
        {
            other.gameObject.BroadcastMessage("addHealth", -_damage);
            Destroy(gameObject);
        }
    }

    public bool outOfBounds()
    {
        return transform.position.x > 22 || 
               transform.position.x < 0 || 
               transform.position.y > 12 || 
               transform.position.y < 0;
    }
}
