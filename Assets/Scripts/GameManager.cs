using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{

    //Singleton attempt
    public static GameManager instance = null;
    public bool GAME_OVER = false;

    public GameObject[] prefabs = new GameObject[2];
    public GameObject HQ1;
    public GameObject HQ2;

    public List<GameObject> team1 = new List<GameObject>();
    public List<GameObject> team2 = new List<GameObject>();
    public List<HoldDragPlaceUnit> deck1 = new List<HoldDragPlaceUnit>();
    public List<HoldDragPlaceUnit> deck2 = new List<HoldDragPlaceUnit>();
    public int[,] tile_validity = new int[22,12];

    public int maxMana = 10;
    public int[] mana;
    public float mana_rate = 3.6f; //Gain 1 mana every n seconds
    private float mana_rate_cooldown;
    

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
        mana[0] = mana[1] = 6;

        foreach (HoldDragPlaceUnit card in deck1)
        {
            card.manaUpdate();
        }

        foreach (HoldDragPlaceUnit card in deck2)
        {
            card.manaUpdate();
        }

        // Instantiate valid tiles for placement.
        // Things like the enemy side, your own towers, should not be valid
        // 0 = no placement allowed
        // 1 = player 1 allowed
        // 2 = player 2 allowed
        // 0 is the default int val in an int array
        for(int j = 0; j < 12; j++)
        {
            // RED SIDE 
            for(int i = 0; i < 10; i++){
                tile_validity[i,j] = 1;
            }

            // GREEN SIDE
            for(int i = 12; i < 22; i++){
                tile_validity[i,j] = 2;
            }


        }

        spawnTowers();
    }

    void Update()
    {
        mana_rate_cooldown += Time.deltaTime;
        team1.RemoveAll(unit => unit == null); //Tried calling from onDestroy() in Unit, doesn't work
        team2.RemoveAll(unit => unit == null);

        if (mana_rate_cooldown > mana_rate)
        {
            mana_rate_cooldown = 0;
            if (mana[0] < maxMana)
            {
                mana[0]++;
                foreach( HoldDragPlaceUnit card in deck1)
                {
                    card.manaUpdate();
                }
            }
            if (mana[1] < maxMana) {
                mana[1]++;
                foreach (HoldDragPlaceUnit card in deck2)
                {
                    card.manaUpdate();
                }
            }
        }

        if (GAME_OVER)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            
        }
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

    private void testFireball()
    {

        if (Input.GetKeyDown("space"))
        {
            List<GameObject> enemys = team1;
            Vector2 center = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            foreach ( GameObject unit in enemys)
            {
                if(Vector2.Distance(center, unit.transform.position) < 1)
                {
                    unit.BroadcastMessage("addHealth", -10);
                    Vector2 knockback = (Vector2)unit.transform.position - center;
                    knockback = knockback.normalized * 100;
                    unit.GetComponent<Rigidbody2D>().AddForce(knockback);
                }
            }
        }
    }

    //To generate starting troops
    private void SetupFieldTest()
    {
        spawn(TEAM.RED, UNIT_NAME.TANK, new Vector2(10, 5));
        spawn(TEAM.GREEN, UNIT_NAME.MTANK, new Vector2(4, 5));
    }

    public void spawnTowers()
    {
        GameObject newTower1 = Instantiate(HQ1, new Vector2(2.5f, 6), Quaternion.identity);
        GameObject newTower2 = Instantiate(HQ2, new Vector2(19.5f, 6), Quaternion.identity);

        team1.Add(newTower1);
        team2.Add(newTower2);
    }

    //For spawning a unit with an assigned team
    public void spawn(TEAM t, UNIT_NAME n, Vector2 spawnPoint)
    {
        int index = (prefabs.Length * (int)t) / 2 + (int)n;
        print(index);
        GameObject newUnit = Instantiate(prefabs[index], spawnPoint, Quaternion.identity);
        newUnit.GetComponent<Unit>()._team = t;
        newUnit.GetComponent<Unit>()._name = n;

        switch (t)
        {
            case TEAM.GREEN:
                team2.Add(newUnit);
                return;

            case TEAM.RED:
                team1.Add(newUnit);
                return;
        }
    }
}


