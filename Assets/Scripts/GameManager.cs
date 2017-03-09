using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour{

    //Singleton attempt
    public static GameManager instance = null;

    public GameObject[] prefabs = new GameObject[2];
    public List<GameObject> team1 = new List<GameObject>();
    public List<GameObject> team2 = new List<GameObject>();
    public GameObject prefab;

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
        SetupField();
    }

    void Update()
    {
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
    public void spawn(GameObject prefab, TEAM t, UNIT_NAME n, Vector2 start)
    {
        GameObject newUnit = Instantiate(prefab, start, Quaternion.identity);
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
