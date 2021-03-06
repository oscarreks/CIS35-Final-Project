﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldDragPlaceUnit : MonoBehaviour {

    public TEAM _team;
    public UNIT_NAME _name;
    public int _numUnits;

    public int _cost;

    public bool enoughMana;
    SpriteRenderer _draggedObject;
    public float _right_bounds, _left_bounds, _top_bounds, _bot_bounds;
    public Sprite _transparency;
    public AudioClip spawn_sound;
    public AudioClip error_sound;

    void Start()
    {
        _draggedObject = new GameObject().AddComponent<SpriteRenderer>();
        _draggedObject.transform.position = transform.position;
        _draggedObject.enabled = false;
        _draggedObject.sprite = _transparency;
        _draggedObject.color = new Color(1, 1, 1, 0.5f);
        _draggedObject.transform.localScale = new Vector3(2, 2, 2);

        //_cost = UnitStats.index[(int)_name].cost;

        manaUpdate();

        _top_bounds = 12;
        _bot_bounds = 0;

        setBounds();
    }

    protected virtual void setBounds()
    {
        if (_team == TEAM.RED)
        {
            _right_bounds = 10;
            _left_bounds = 0;
        }
        else
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


    protected Vector2 CurrentTouchPosition
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    void OnMouseDown()
    {
        _draggedObject.enabled = true;
    }

    private void OnMouseDrag()
    {
        _draggedObject.transform.position = snapToGrid(CurrentTouchPosition);

    }

    private void OnMouseUp()
    {
        if (validPlacement() && enoughMana)
        {
            spawnUnit();
        }
        else
        {
            SoundManager.instance.Play(error_sound);
        }

        _draggedObject.enabled = false;
        _draggedObject.transform.position = transform.position;
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

    protected virtual void spawnUnit()
    {
        SoundManager.instance.Play(spawn_sound);
        
        GameManager.instance.mana[(int)_team] -= _cost;
        manaUpdate();
        Vector2 gridPos = snapToGrid(CurrentTouchPosition);

        switch(_numUnits)
        {
            case 1:
                GameManager.instance.spawn(_team, _name, gridPos);
                return;
            case 2:
                GameManager.instance.spawn(_team, _name, new Vector2(gridPos.x, gridPos.y + .3f));
                GameManager.instance.spawn(_team, _name, new Vector2(gridPos.x, gridPos.y - .3f));
                return;
            case 3:
                GameManager.instance.spawn(_team, _name, new Vector2(gridPos.x, gridPos.y + .2f));
                GameManager.instance.spawn(_team, _name, new Vector2(gridPos.x, gridPos.y - .2f));
                GameManager.instance.spawn(_team, _name, new Vector2(gridPos.x + 0.1f, gridPos.y));
                return;
            default:
                GameManager.instance.spawn(_team, _name, gridPos);
                return;
        }

    }

    public Vector2 snapToGrid(Vector2 pos)
    {
        float x = Mathf.Clamp(Mathf.CeilToInt(pos.x), _left_bounds + 1, _right_bounds) - 0.5f;
        float y = Mathf.Clamp(Mathf.CeilToInt(pos.y), _bot_bounds + 1, _top_bounds) - 0.5f;
        return new Vector2(x, y);
    }

}
