using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpriteManager : MonoBehaviour {

    public Sprite sprite;
    SpriteRenderer sr;
    public int face_state = 0;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float face = GetComponentInParent<Transform>().localEulerAngles.z;
        float mod = Mathf.Abs(face % 360);

        //IN the future maybe do (face/180)->convert to int
        if(mod < 90 || mod > 270)
        {
            face_state = 0;
        }
        else
        {
            face_state = 1;
        }

        switch (face_state)
        {
            case 0:
                sr.flipX = false;
                break;
            case 1:
                sr.flipX = true;
                break;
        }
    }

    void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }

}
