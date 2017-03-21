using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{

    //Singleton attempt
    public static GameManager instance = null;

    public GameObject[] prefabs = new GameObject[2];
    public List<GameObject> team1 = new List<GameObject>();
    public List<GameObject> team2 = new List<GameObject>();
    public GameObject HQ1;
    public GameObject HQ2;
    public GameObject prefab;

    public int mana1;
    public int mana2;
    public float mana_rate = 2.0f; //Gain 1 mana every n seconds
    public float mana_rate_cooldown;
    


    void Awake()
    {
        if(instance == null) //if this object has not been instantiated yet, assign it the global status
        {
            instance = this;
        }else if(instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //prefabs = Resources.LoadAll("UnitPrefabs") as GameObject[];
        Physics2D.gravity = Vector2.zero;
        SetupField();

        mana1 = mana2 = 3;
    }

    void Update()
    {
        mana_rate_cooldown += Time.deltaTime;

        if (mana_rate_cooldown > mana_rate)
        {
            mana_rate_cooldown = 0;
            if (mana1 < 10) { mana1++; }
            if (mana2 < 10) { mana2++; }
        }

        if (Input.GetMouseButtonDown(0))
        {
            
            var mousex = Input.mousePosition.x;
            var mousey = Input.mousePosition.y;
            var ray = Camera.main.ScreenPointToRay(new Vector3(mousex, mousey, 0));

            spawn(prefabs[0], TEAM.RED, UNIT_NAME.TANK, ray.origin);
        }
        else if (Input.GetMouseButtonDown(1))
        {

            var mousex = Input.mousePosition.x;
            var mousey = Input.mousePosition.y;
            var ray = Camera.main.ScreenPointToRay(new Vector3(mousex, mousey, 0));

            spawn(prefabs[1], TEAM.GREEN, UNIT_NAME.MTANK, ray.origin);
        }
    }

    //To generate starting troops
    private void SetupField()
    {
        spawn(prefabs[0], TEAM.RED, UNIT_NAME.TANK, new Vector2(-5.0f, 0));
        spawn(prefabs[1], TEAM.GREEN, UNIT_NAME.MTANK, new Vector2(5.0f, 0));
    }

    //For spawning a unit with an assigned team
    public void spawn(GameObject prefab, TEAM t, UNIT_NAME n, Vector2 spawnPoint)
    {
        GameObject newUnit = Instantiate(prefab, spawnPoint, Quaternion.identity);
        newUnit.GetComponent<Unit>()._team = t;
        newUnit.GetComponent<Unit>()._name = n;

        if (t == TEAM.GREEN)
        {
            team2.Add(newUnit);
            return;
        }
        if(t == TEAM.RED)
        {
            team1.Add(newUnit);
            return;
        }
    }
}


