using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldDragPlaceUnit : MonoBehaviour {

    public TEAM _team;
    public UNIT_NAME _name;

    private int _cost;

    bool enoughMana = false;
    SpriteRenderer _draggedObject;

    void Start()
    {
        GameObject temp = GameManager.instance.prefabs[(int)_name];
        _draggedObject = new GameObject().AddComponent<SpriteRenderer>();
        _draggedObject.transform.position = transform.position;
        _draggedObject.enabled = false;
        _draggedObject.sprite = temp.GetComponent<SpriteRenderer>().sprite;
        _draggedObject.color = new Color(1, 1, 1, 0.5f);

        _cost = UnitStats.index[(int)_name].cost;
    }


    bool hasInput()
    {
        return Input.GetMouseButton(0);
    }

    Vector2 CurrentTouchPosition
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    void OnMouseDown()
    {
        print("TOUCHED ME");
        if (GameManager.instance.mana[(int)_team] >= _cost)
        {
            enoughMana = true;
            _draggedObject.enabled = true;
        }
    }

    private void OnMouseDrag()
    {
        if (enoughMana)
        {
            float x = Mathf.CeilToInt(CurrentTouchPosition.x) - 0.5f;
            float y = Mathf.CeilToInt(CurrentTouchPosition.y) - 0.5f;
            _draggedObject.transform.position = new Vector2(x, y);

        }
    }

    private void OnMouseUp()
    {
        if (enoughMana)
        {
            if (validPlacement())
            {
                enoughMana = false;
                spawnUnit();
            }else
            {
                _draggedObject.enabled = false;
                _draggedObject.transform.position = transform.position;
                print("YOU CANT PUT THAT THERE");
            }
        }
    }

    private bool validPlacement()
    {
        return true;
    }

    private void spawnUnit()
    {
        _draggedObject.enabled = false;
        _draggedObject.transform.position = transform.position;
        GameManager.instance.spawn(_team, _name, CurrentTouchPosition);
        GameManager.instance.mana[(int)_team] -= _cost;
    }

}
