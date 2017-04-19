using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Unit is the base class for the game units, but does not handle movment or attacking.
/// </summary>
/// TODO - Cut down this class to the core, move the rest to specialized extensions 
/// KEEP - addHealth, DetermineTarget, FaceObject, pulseRed, startDeath, stopMoving
/// MOVE - moveTowardsTarget, lerpProjectile, targetInRange

public abstract class Unit : MonoBehaviour
{

    public UNIT_NAME _name;
    public TEAM _team;

    public GameObject target;
    public Vector2 target_coords;
    public float _attack, _health, _speed, _cost, _range, _attack_speed;
    public float _fire_cooldown;
    public GameObject bullet_prefab;

    //ROUGH AUDIO IMPLEMENTATION
    public AudioClip damage_sound;
    public AudioClip firing_sound;
    public AudioSource audio_source;

    protected List<GameObject> _enemyTeam;
    protected GameObject _enemyHQ;
    protected Rigidbody2D _rb;

    private bool targetingHQ;
    protected bool isMoving;


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

        //target = _enemyHQ;
        //targetingHQ = true;
        isMoving = true; //A bit misleading; it means addForce() is not being called
        SetPath();      //Set up path for unit type
        DetermineTarget();
        _rb = GetComponent<Rigidbody2D>();
        audio_source = GetComponent<AudioSource>();
    }

    void Update()
    {
        _fire_cooldown += Time.deltaTime;
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(transform.position.x, Screen.height - transform.position.y, 100, 100), "" + _health);
    }

    void FixedUpdate()
    {
        DetermineTarget();          //virtual

        if (TargetInRange())
        {
            FaceTarget();
            if (isMoving)
                stopMoving();

            damageTarget();         //abstract
        }
        else
        {
            moveTowardsTarget();    //abstract
        }

        if (_health < 0)
        {
            Destroy(gameObject);    //maybe virtual
        }
    }



    // ---- MOVING AND TARGETING FUNCTIONS ----

    /// <summary>
    /// Returns true if target position is within range, defined by _range
    /// </summary>
    /// <returns>true if its da true true</returns>
    protected bool TargetInRange()
    {
        if(target == null) { return false; }
        return Vector2.Distance(transform.position, target_coords) < _range;
        
    }

    // ---- CORE FUNCTIONS ----

    /// <summary>
    /// Faces this object towards the target mem
    /// </summary>
    protected void FaceTarget()
    {
        if (target) { target_coords = target.transform.position; }
        Vector2 direction = target_coords - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }


    /// <summary>
    /// Adds health to local health. Use a negative float to take away health
    /// </summary>
    /// <param name="health"></param>
    public void addHealth(float health)
    {
        _health += health;

        StopCoroutine(pulseRed());
        StartCoroutine(pulseRed());

        //V------------------------and dis is where I play the sounds for now
        SoundManager.instance.Play(damage_sound);
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



    // ---- VIRTUAL & ABSTRACT FUNCTIONS ----

    /// <summary>
    /// Called in Start(), should be overridden to allow proper pathing
    /// </summary>
    protected virtual void SetPath()
    {
        print("DEFAULT SETPATH TRIGGERED");
    }

    /// <summary>
    /// Apply damage to target
    /// </summary>
    protected abstract void damageTarget();

    /// <summary>
    /// Moves this object towards the target position, using Addforce.
    /// Velocity is clamped at _speed
    /// </summary>
    protected abstract void moveTowardsTarget();

    /// <summary>
    /// Determines if last target is gone, or if certain things can be targeted
    /// </summary>
    protected virtual void DetermineTarget()
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


}
