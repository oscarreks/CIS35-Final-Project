using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct node
{
    public Vector2 position;
    public float distToEnd;
    //public int[] neighbors;

    public node(Vector2 p, float d)
    {
        position = p;
        distToEnd = d;
        //neighbors = n;
    }
}

public class GroundUnit : Unit {

    public int next_node;
    public int side_index = 1;
    public Vector2[][] path;

    /*
    static int[][] adj_ltr = new int[][]
    {
        new int[] { 0, -1, -1, 10 },
        new int[] { -1, 0, 10, -1 },
        new int[] { -1, 10, 0, 7 },
        new int[] { 10, -1, 7, 0 }
    };
    */

    static Vector2[][] path_rightwards = new Vector2[][]
    {
        new Vector2[]
        {
            new Vector2(9.5f, 9.5f),
            new Vector2(19.5f, 9.5f),
            new Vector2(19.5f, 6)
        },
        new Vector2[]
        {
            new Vector2(9.5f, 2.5f),
            new Vector2(19.5f, 2.5f),
            new Vector2(19.5f, 6)
        }
    };

    static Vector2[][] path_leftwards = new Vector2[][]
    {
        new Vector2[]
        {
            new Vector2(12.5f, 9.5f),
            new Vector2(2.5f, 9.5f),
            new Vector2(2.5f, 6)
        },
        new Vector2[]
        {
            new Vector2(12.5f, 2.5f),
            new Vector2(2.5f, 2.5f),
            new Vector2(2.5f, 6)
        }

    };


    protected override void SetPath()
    {
        if(_team == TEAM.RED)
        {
            path = path_rightwards;
        }
        else
        {
            path = path_leftwards;
        }

        if(transform.position.y > 6)
        {
            side_index = 0;
        }
    }

    /// <summary>
    /// Does damage its target; damage delay is relative to target distance
    /// </summary>
    protected override void damageTarget()
    {
        //anim.SetBool("Firing", _fire_cooldown > _attack_speed);
        if (_fire_cooldown > _attack_speed)
        {

            target.BroadcastMessage("addHealth", -_attack);
            _fire_cooldown = 0;

            //anim.SetTrigger("Fired");

            SoundManager.instance.Play(firing_sound, 0.3f);
        }
    }

    //This probably isn't needed unless the distance is extreme
    private IEnumerator delayDamage(float delay)
    {
        yield return new WaitForSeconds(delay);
        target.BroadcastMessage("addHealth", -_attack);
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
                    //target = closestEnemy()
                    target = enemy;
                    target_coords = target.transform.position;
                    return;
                }
            }
            //Assuming we've reached this point, no enemies are within sight_radius
            target_coords = path[side_index][next_node];
        }

    }

    //prototype
    //Eventually want to be able to find fastest path to end target
    private void checkNode()
    {
        if(transform.position.y > 6) // top
        {
            side_index = 0;
        }
        else
        {
            side_index = 1;
        }

        if(Vector2.Distance(transform.position, path[side_index][next_node]) < 0.9f && next_node < path.Length)
        {
            next_node++;
        }
        
    }
}
