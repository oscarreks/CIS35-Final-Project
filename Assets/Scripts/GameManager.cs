using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{

    //Singleton attempt
    public static GameManager instance = null;

    public GameObject[] prefabs = new GameObject[2];
    public List<GameObject> team1 = new List<GameObject>();
    public List<GameObject> team2 = new List<GameObject>();
    public List<HoldDragPlaceUnit> deck1 = new List<HoldDragPlaceUnit>();
    public List<HoldDragPlaceUnit> deck2 = new List<HoldDragPlaceUnit>();
    public GameObject HQ1;
    public GameObject HQ2;
    public GameObject prefab;

    public int[] mana;
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
        //SetupFieldTest();

        mana = new int[2];
        mana[0] = mana[1] = 3;
    }

    void Update()
    {
        mana_rate_cooldown += Time.deltaTime;

        if (mana_rate_cooldown > mana_rate)
        {
            mana_rate_cooldown = 0;
            if (mana[0] < 10)
            {
                mana[0]++;
                foreach( HoldDragPlaceUnit card in deck1)
                {
                    card.manaUpdate();
                }
            }
            if (mana[1] < 10) {
                mana[1]++;
                foreach (HoldDragPlaceUnit card in deck2)
                {
                    card.manaUpdate();
                }
            }
        }

        //testSpawning();

    }


    //For testing the spawn() function
    private void testSpawning()
    {
        if (Input.GetMouseButtonDown(0))
        {

            var mousex = Input.mousePosition.x;
            var mousey = Input.mousePosition.y;
            var ray = Camera.main.ScreenPointToRay(new Vector3(mousex, mousey, 0));

            spawn(TEAM.RED, UNIT_NAME.TANK, ray.origin);
        }
        else if (Input.GetMouseButtonDown(1))
        {

            var mousex = Input.mousePosition.x;
            var mousey = Input.mousePosition.y;
            var ray = Camera.main.ScreenPointToRay(new Vector3(mousex, mousey, 0));

            spawn(TEAM.GREEN, UNIT_NAME.MTANK, ray.origin);
        }
    }

    /*
    private void OnGUI()
    {
        GUI.Label(new Rect(0, 0, 100, 100), "Red's Mana: " + mana[0]);
        GUI.Label(new Rect(0, 20, 100, 100), "Green's Mana: " + mana[1]);
    }
    */

    //To generate starting troops
    private void SetupFieldTest()
    {
        spawn(TEAM.RED, UNIT_NAME.TANK, new Vector2(10, 5));
        spawn(TEAM.GREEN, UNIT_NAME.MTANK, new Vector2(4, 5));
    }

    //For spawning a unit with an assigned team
    public void spawn(TEAM t, UNIT_NAME n, Vector2 spawnPoint)
    {
        GameObject newUnit = Instantiate(prefabs[(int)n], spawnPoint, Quaternion.identity);
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


