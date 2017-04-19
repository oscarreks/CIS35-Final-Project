using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldDragPlaceUnit : MonoBehaviour {

    public TEAM _team;
    public UNIT_NAME _name;

    private int _cost;

    public bool enoughMana;
    SpriteRenderer _draggedObject;
    public float _right_bounds, _left_bounds, _top_bounds, _bot_bounds;
    public AudioClip spawn_sound;

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

        manaUpdate();

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

    public void manaUpdate()
    {
        enoughMana = GameManager.instance.mana[(int)_team] >= _cost;
        if (enoughMana)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }else
        {
            GetComponent<SpriteRenderer>().color = Color.gray;
        }
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
        if (enoughMana)
        {
            _draggedObject.enabled = true;
        }
    }

    private void OnMouseDrag()
    {
        if (enoughMana)
        {

            float x = Mathf.Clamp(Mathf.CeilToInt(CurrentTouchPosition.x), _left_bounds + 1, _right_bounds) - 0.5f;
            float y = Mathf.Clamp(Mathf.CeilToInt(CurrentTouchPosition.y), _bot_bounds + 1, _top_bounds) - 0.5f;
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
            }

            _draggedObject.enabled = false;
            _draggedObject.transform.position = transform.position;
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
        SoundManager.instance.Play(spawn_sound);
        GameManager.instance.spawn(_team, _name, CurrentTouchPosition);
        GameManager.instance.mana[(int)_team] -= _cost;
        manaUpdate();
    }

}
