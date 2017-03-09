using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    public UNIT_NAME _name;
    public TEAM _team;

    private float _fire_cooldown; 
    public GameObject target;
    public float _attack, _health, _speed, _cost, _range, _attack_speed;
    public GameObject bullet_prefab;

    private List<GameObject> _enemyTeam;


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
        //grab appropriate sprite/prefab/whatever

        if (_team == TEAM.RED)
            _enemyTeam = GameManager.instance.team2;

        if (_team == TEAM.GREEN)
            _enemyTeam = GameManager.instance.team1;

        DetermineTarget();
    }


    void Update()
    {
        _fire_cooldown += Time.deltaTime;

        if (TargetInRange())
        {
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("I'M HIT");
    }


    /*
        Moves the unit towards its target; if none in range, heads towards tower
    */
    private void moveTowardsTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, _speed * Time.deltaTime);
    }



    private void DetermineTarget()
    {
        target = _enemyTeam[0];
        /*
        if (true) {
            print("true");//target = getenemy.transform should be closest enemy too
        } else {
            //target = closest tower
            print("switching targets");
        }
        */
    }

    
    private bool TargetInRange()
    {
        return Vector2.Distance(transform.position, target.transform.position) < _range;
    }


    // Fires a Bullet gameobject towards the coords of the enemies
    private void fireTowardsTarget()
    {
        GameObject newBullet = Instantiate(bullet_prefab, transform.position, Quaternion.identity);
        newBullet.GetComponent<MoveBullet>().target = target.transform.position;
        newBullet.GetComponent<MoveBullet>()._team = _team;
    }

    private void startDeath()
    {
        //Call on Destroy(), update the GameManager's arrays.
        Destroy(gameObject);
    }

    public void addHealth(float hp)
    {
        print("IVE BEEN HIT");
        _health += hp;
    }



}
