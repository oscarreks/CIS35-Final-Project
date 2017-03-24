using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldDragPlaceUnit : MonoBehaviour {

    public TEAM _team;
    public UNIT_NAME _name;

    private int _cost;

    bool enoughMana = false;
    SpriteRenderer _draggedObject;
    public float _right_bounds, _left_bounds, _top_bounds, _bot_bounds;

    void Start()
    {
        GameObject temp = GameManager.instance.prefabs[(int)_name];
        _draggedObject = new GameObject().AddComponent<SpriteRenderer>();
        _draggedObject.transform.position = transform.position;
        _draggedObject.enabled = false;
        _draggedObject.sprite = temp.GetComponent<SpriteRenderer>().sprite;
        _draggedObject.color = new Color(1, 1, 1, 0.5f);
        _draggedObject.transform.localScale = new Vector3(2, 2, 2);

        _cost = UnitStats.index[(int)_name].cost;

        _top_bounds = 12;
        _bot_bounds = 0;

        if(_team == TEAM.RED)
        {
            _right_bounds = 10;
            _left_bounds = 0;
        }else
        {
            _right_bounds = 22;
            _left_bounds = 12;
        }
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
            if (validPlacement())
            {
                float x = Mathf.CeilToInt(CurrentTouchPosition.x) - 0.5f;
                float y = Mathf.CeilToInt(CurrentTouchPosition.y) - 0.5f;
                _draggedObject.transform.position = new Vector2(x, y);
            }
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
            }
        }
    }

    private bool validPlacement()
    {
        if (CurrentTouchPosition.x > _right_bounds ||
            CurrentTouchPosition.x < _left_bounds  ||
            CurrentTouchPosition.y > _top_bounds   ||
            CurrentTouchPosition.y < _bot_bounds)
        {
            return false;
        }
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
