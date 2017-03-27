using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct node
{
    public Vector2 position;
    public float distToEnd;

    public node(Vector2 p, float d)
    {
        position = p;
        distToEnd = d;
    }
}

public class GroundUnit : Unit {

    public int next_node;

    node[] path =
    {
        new node(new Vector2(9.5f, 9.5f), 100),
        new node(new Vector2(9.5f, 2.5f), 150),
        new node(new Vector2(19.5f, 2.5f), 50),
        new node(new Vector2(19.5f, 9.5f), 0)
    };

    /// <summary>
    /// Does damage its target; damage delay is relative to target distance
    /// </summary>
    protected override void damageTarget()
    {
        if (_fire_cooldown > _attack_speed)
        {
            //TODO - tear this out, just do straight damage, delayed with a coroutine
            GameObject newBullet = Instantiate(bullet_prefab, transform.position, transform.rotation);
            newBullet.GetComponent<MoveBullet>()._team = _team;
            _fire_cooldown = 0;

            //float dist = Vector2.Distance(transform.position, target.transform.position);

        }
    }

    /// <summary>
    /// Moves this object towards the target member, using Addforce.
    /// Velocity is clamped at _speed
    /// </summary>
    protected override void moveTowardsTarget()
    {
        isMoving = true;
        checkNode();
        FaceTarget();
        if (_rb.velocity.magnitude < _speed)
            _rb.AddRelativeForce(new Vector2(_speed, 0));  //In the future, to account for mass, this might be _speed*mass
    }

    protected override void DetermineTarget()
    {
        if (target == null)
        {
            foreach (GameObject enemy in _enemyTeam) //TODO swap _range with _sight_radius
            {
                if (enemy != null && Vector2.Distance(transform.position, enemy.transform.position) <= UnitStats.sight_radius) 
                {
                    //In the future, might need to iterate through all to find CLOSEST enemy
                    target = enemy;
                    target_coords = target.transform.position;
                    return;
                }
            }
            //Assuming we've reached this point, no enemies are within sight_radius
            target_coords = path[next_node].position;
        }

    }

    //prototype
    private void checkNode()
    {
        if(Vector2.Distance(transform.position, path[next_node].position) < 0.5f && next_node < path.Length)
        {
            next_node++;
        }
    }
}
