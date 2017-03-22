using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public UNIT_NAME _name;
    public TEAM _team;

    public GameObject target;
    public float _attack, _health, _speed, _cost, _range, _attack_speed;
    public float _fire_cooldown;
    public GameObject bullet_prefab;
    public GameObject healthbar_prefab;
    private GameObject healthbar;
    public float health_distance = 2.0f;

    private List<GameObject> _enemyTeam;
    private GameObject _enemyHQ;
    private Rigidbody2D _rb;
    private ConstantForce2D _cf;

    private bool targetingHQ;


    //Reference some default target in the gameManager.
    //also grab appropriate player list for enemies.


    void Start()
    {
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
        DetermineTarget();
        _rb = GetComponent<Rigidbody2D>();
        //_cf = GetComponent<ConstantForce2D>();
        //_cf.relativeForce = new Vector2(_speed, 0);

        //making the healthbar
        healthbar = Instantiate(healthbar_prefab, transform.position, Quaternion.identity);

    }

    void Update()
    {
        //healthbar.transform.position = transform.position + new Vector3(0, health_distance, 0);
        //healthbar.GetComponent<HealthBar>().health = _health;
    }

    void FixedUpdate()
    {
        _fire_cooldown += Time.deltaTime;
        DetermineTarget();

        if (TargetInRange())
        {
            stopMoving();
            if (_fire_cooldown > _attack_speed)
            {
                fireTowardsTarget();
                _fire_cooldown = 0;
            }
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
            foreach (GameObject enemy in _enemyTeam)
            {
                if (enemy != null && Vector2.Distance(transform.position, enemy.transform.position) <= _range)
                {
                    target = enemy; //Instance or reference?
                    targetingHQ = false;
                    return;
                }
            }

            target = _enemyHQ;
            targetingHQ = true;
        }

    }


    private void moveTowardsTarget()
    {
        transform.rotation = FaceObject(transform.position, target.transform.position);
        //Vector2 move_dir = target.transform.position - transform.position;
        //_rb.velocity = move_dir.normalized * _speed;
        if (_rb.velocity.magnitude < _speed)
            _rb.AddRelativeForce(new Vector2(_speed, 0));
    }


    /*
    Will create a bullet object and fire it towards the target's position
*/
    private void fireTowardsTarget()
    {
        transform.rotation = FaceObject(transform.position, target.transform.position);
        GameObject newBullet = Instantiate(bullet_prefab, transform.position, transform.rotation);
        newBullet.GetComponent<MoveBullet>()._team = _team;
    }

    //DetermineTarget() should make sure "target" always exists;
    private bool TargetInRange()
    {
        return Vector2.Distance(transform.position, target.transform.position) < _range;
        
    }

    //Faces the given target, starting position will usually be this transform
    public Quaternion FaceObject(Vector2 startingPosition, Vector2 targetPosition)
    {
        Vector2 direction = targetPosition - startingPosition;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }

    
    // ---- LESS IMPORTANT FUNCTIONS ----


    private void startDeath()
    {
        //Call on Destroy(), update the GameManager's arrays.
        Destroy(gameObject);
    }

    public void addHealth(float h)
    {
        _health += h;
        print("IVE BEEN HIT");
        
        StopCoroutine(pulseRed());
        StartCoroutine(pulseRed());
    }

    private IEnumerator pulseRed()
    {
        /*
        for(int i = 255; i > 0; i -= 5){
            GetComponent<SpriteRenderer>().color = new Color(i, 0, 0);
            yield return null;
        }*/
        print("Current Color: " + GetComponent<SpriteRenderer>().color);
        float red = 0.5f;
        GetComponent<SpriteRenderer>().color = new Color(red, 1, 1);
        yield return new WaitForSeconds(.1f);
        while(red < 1.0f)
        {
            GetComponent<SpriteRenderer>().color = new Color(red, 1, 1);
            red += 0.02f;
            yield return null;
        }
    }

    private void stopMoving()
    {
        //_cf.relativeForce = Vector2.zero;
        _rb.velocity = Vector2.zero;
    }

}
