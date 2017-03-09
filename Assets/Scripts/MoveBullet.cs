using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBullet : MonoBehaviour {

    public TEAM _team;
    public float _speed = 5.0f;
    public float _damage = 10.0f;
    public Vector2 target;  //Assign the target
    public Vector3 movementVector;

    void Start()
    {
        movementVector = target - (Vector2)transform.position;
        movementVector.Normalize();
    }

	void Update () {
        transform.position += movementVector * Time.deltaTime * _speed;
        if (outOfBounds())
        {
            print("I WENT OUR OF BOUNDS");
            Destroy(gameObject);
        }
    }


    void OnCollisionEnter2D(Collision2D col)
    {
        print("MY COORDS ARE" + transform.position);

        if (col.gameObject.GetComponent<Unit>()._team != _team)
        {
            col.gameObject.SendMessage("addHealth", _damage);
            print("I DID DAMAGE");
            Destroy(gameObject);
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
