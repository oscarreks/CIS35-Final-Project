//NOT BEING USED ANYMORE. DON'T APPLY CHANGES

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour {

    public UNIT_NAME _name;
    public TEAM team;

    public bool isMoving = true;
    public GameObject target;
    public float _attack, _health, _speed, _cost, _range;

    private List<GameObject> _enemyTeam;


    //Reference some default target in the gameManager.
    //also grab appropriate player list for enemies.
    

    void Start() {
        _attack = UnitStats.index[(int)_name].attack;
        _health = UnitStats.index[(int)_name].health;
        _speed = UnitStats.index[(int)_name].speed;
        _cost = UnitStats.index[(int)_name].cost;
        _range = UnitStats.index[(int)_name].range;
        //grab appropriate sprite/prefab/whatever

        if(team == TEAM.RED)
            _enemyTeam = GameManager.instance.team2;

        if (team == TEAM.GREEN)
            _enemyTeam = GameManager.instance.team1;

        DetermineTarget();
    }


    void Update()
    {
        if(TargetInRange()){
            attackTarget();
        }
        else{
            moveTowardsTarget();
        }

        if(_health < 0)
        {
            startDeath();
        }
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

    private void attackTarget()
    {
        target = _enemyTeam[0];
    }

    private void startDeath()
    {
        //Call on Destroy(), update the GameManager's arrays.
    }


}
