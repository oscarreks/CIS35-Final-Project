using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour {

    public TEAM _team;
    public float _health = 1200;
    public float _attack = 40;
    public float _attack_speed = 2f;
    public float _fire_cooldown;

    public GameObject target;
    private List<GameObject> _enemy_team;

    public AudioClip damage_sound;
    public AudioClip firing_sound;

    void Start()
    {
        BroadcastMessage("initHealth", _health);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y / 12);

        if(_team == TEAM.GREEN)
        {
            _enemy_team = GameManager.instance.team1;
        }
        else
        {
            _enemy_team = GameManager.instance.team2;
        }
    }

    void Update()
    {
        _fire_cooldown += Time.deltaTime;

        DetermineTarget();

        if (TargetInRange())
        {
            damageTarget();
        }

        if(_health < 0)
        {
            startDeath();
        }
    }

    public void addHealth(float health)
    {
        _health += health;
        print(gameObject.name + ": " + _health);
    }

    private void damageTarget()
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

    private bool TargetInRange()
    {
        if (target == null) { return false; }
        return Vector2.Distance(transform.position, target.transform.position) < UnitStats.sight_radius;
    }

    private void DetermineTarget()
    {
        if (target == null)
        {
            foreach (GameObject enemy in _enemy_team)
            {
                if (enemy != null && Vector2.Distance(transform.position, enemy.transform.position) <= UnitStats.sight_radius)
                {
                    //In the future, might need to iterate through all to find CLOSEST enemy
                    //target = closestEnemy()
                    target = enemy;
                    return;
                }
            }
        }

    }

    public void startDeath()
    {
        //make cool death animation
        Destroy(gameObject);
        GameManager.instance.GAME_OVER = true;
    }
}
