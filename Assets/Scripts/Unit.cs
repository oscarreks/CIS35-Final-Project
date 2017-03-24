using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unit is the base class for the game units, but does not handle movment or attacking.
/// </summary>
///TODO - Cut down this class to the core, move the rest to specialized extensions 
/// KEEP - addHealth, DetermineTarget, FaceObject, pulseRed, startDeath, stopMoving
/// MOVE - moveTowardsToarget, lerpProjectile, targetInRange

public class Unit : MonoBehaviour
{

    public UNIT_NAME _name;
    public TEAM _team;

    public GameObject target;
    public Vector2 target_coords;
    public float _attack, _health, _speed, _cost, _range, _attack_speed;
    public float _fire_cooldown;
    public GameObject bullet_prefab;

    private List<GameObject> _enemyTeam;
    private GameObject _enemyHQ;
    private Rigidbody2D _rb;

    private bool targetingHQ;
    private bool isMoving;


    void Start()
    {

        //TODO move all this to an init loop inside GameManager
        _attack = UnitStats.index[(int)_name].attack;
        _health = UnitStats.index[(int)_name].health;
        _speed = UnitStats.index[(int)_name].speed;
        _cost = UnitStats.index[(int)_name].cost;
        _range = UnitStats.index[(int)_name].range;
        _attack_speed = UnitStats.index[(int)_name].attack_speed;

        if (_team == TEAM.RED)
        {
            _enemyTeam = GameManager.instance.team2;
            _enemyHQ = GameManager.instance.HQ2;
        }

        if (_team == TEAM.GREEN)
        {
            _enemyTeam = GameManager.instance.team1;
            _enemyHQ = GameManager.instance.HQ1;
        }

        target = _enemyHQ;
        targetingHQ = true;
        isMoving = true; //A bit misleading; it means addForce() is not being called
        DetermineTarget();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _fire_cooldown += Time.deltaTime;
    }

    void FixedUpdate()
    {
        DetermineTarget();

        if (TargetInRange())
        {
            FaceTarget();
            if (isMoving)
                stopMoving();

            fireTowardsTarget();
        }
        else
        {
            moveTowardsTarget();
        }

        if (_health < 0)
        {
            startDeath();
        }
    }



    // ---- MOVING AND TARGETING FUNCTIONS ----

    /*
        If no targets are within range, it will set target to closest tower
    */
    private void DetermineTarget()
    {
        if (target == null || targetingHQ)
        {
            foreach (GameObject enemy in _enemyTeam) //TODO swap _range with _sight_radius
            {
                if (enemy != null && Vector2.Distance(transform.position, enemy.transform.position) <= UnitStats.sight_radius)
                {
                    target = enemy;
                    targetingHQ = false;
                    return;
                }
            }

            target = _enemyHQ;
            targetingHQ = true;
        }

    }

 
    /// <summary>
    /// Moves this object towards the target member, using Addforce.
    /// Velocity is clamped at _speed
    /// </summary>
    private void moveTowardsTarget()
    {
        isMoving = true;
        FaceTarget();
        if (_rb.velocity.magnitude < _speed)
            _rb.AddRelativeForce(new Vector2(_speed, 0));  //In the future, to account for mass, this might be _speed*mass
    }


    /// <summary>
    /// Does damage its target; damage delay is relative to target distance
    /// </summary>
    private void fireTowardsTarget()
    {
        if (_fire_cooldown > _attack_speed)
        {
            GameObject newBullet = Instantiate(bullet_prefab, transform.position, transform.rotation);
            newBullet.GetComponent<MoveBullet>()._team = _team;
            _fire_cooldown = 0;

            float dist = Vector2.Distance(transform.position, target.transform.position);

        }
    }

    /* 
     * DetermineTarget() should make sure "target" always exists;
     * Will take the unit's "weapon" range, and initiate fire
     * 
     */
    private bool TargetInRange()
    {
        return Vector2.Distance(transform.position, target.transform.position) < _range;
        
    }

    // ---- CORE FUNCTIONS ----

    /// <summary>
    /// Returns a Quaternion with startingPosition as the origin, facing towards the target position
    /// </summary>
    /// <param name="startingPosition">The origin</param>
    /// <param name="targetPosition">The target coord</param>
    /// <returns></returns>
    /*
    public Quaternion FaceObject(Vector2 startingPosition, Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - startingPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }
    */

    private void FaceTarget()
    {
        Vector2 direction = target.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }


    /// <summary>
    /// Called when health is less than zero
    /// </summary>
    private void startDeath()
    {
        //Call on Destroy(), update the GameManager's arrays.
        Destroy(gameObject);
    }

    /// <summary>
    /// Adds health to local health. Use a negative float to take away health
    /// </summary>
    /// <param name="health"></param>
    public void addHealth(float health)
    {
        _health += health;
        print("IVE BEEN HIT");
        
        StopCoroutine(pulseRed());
        StartCoroutine(pulseRed());
    }

    private IEnumerator pulseRed()
    {
        float red = 0.5f;
        GetComponent<SpriteRenderer>().color = new Color(red, 1, 1);
        yield return new WaitForSeconds(.1f);
        while(red < 1.0f)
        {
            GetComponent<SpriteRenderer>().color = new Color(red, 1, 1);
            red += 0.02f; //this affects the pulse speed
        }
    }

    public IEnumerator lerpProjectile(Vector2 start, Vector2 end)
    {
        yield return null;
    }

    private void stopMoving()
    {
        _rb.velocity = Vector2.zero;
        isMoving = false;
    }



}
