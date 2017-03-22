using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldDragPlaceUnit : MonoBehaviour {

    public Sprite _button;
    public Sprite _dragged;
    public TEAM _team;
    public UNIT_NAME _name;

    private Vector2 inputPosition;
    private float cost;

    bool dragging = false;
    GameObject draggedObject;
    Vector2 touchOffset;

    bool hasInput()
    {
        return Input.GetMouseButton(0);
    }

    Vector2 CurrentTouchPosition
    {
        get
        {
            Vector2 inputPos;
            inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return inputPos;
        }
    }

    void dragOrPickup()
    {
        inputPosition = CurrentTouchPosition;
        if (dragging)
        {
            draggedObject.transform.position = inputPosition + touchOffset;
        }
        else
        {
            RaycastHit2D[] touches = Physics2D.RaycastAll(inputPosition, inputPosition, 0.5f);
            if (touches.Length > 0)
            {
                var hit = touches[0];
                if (hit.transform != null)
                {
                    dragging = true;
                    draggedObject = hit.transform.gameObject;
                    touchOffset = (Vector2)hit.transform.position - inputPosition;
                    draggedObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                }

            }
        }
    }

    void spawnUnit()
    {
        dragging = false;
        GameManager.instance.spawn(_team, _name, inputPosition);
    }

    // Uncomment to get dragdrop working again
    /*
	void Update () {
        if (hasInput())
        {
            dragOrPickup();
        }
        else if(draggingItem)
        {
            dropItem();
        }
	}
    */
}
