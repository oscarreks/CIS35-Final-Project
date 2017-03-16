using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDrop : MonoBehaviour {

    bool draggingItem = false;
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
        var inputPosition = CurrentTouchPosition;
        if (draggingItem)
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
                    draggingItem = true;
                    draggedObject = hit.transform.gameObject;
                    touchOffset = (Vector2)hit.transform.position - inputPosition;
                    draggedObject.transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                }

            }
        }
    }

    void dropItem()
    {
        draggingItem = false;
        draggedObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
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
