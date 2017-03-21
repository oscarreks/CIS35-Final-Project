using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour {

    public SpriteRenderer bar;
    public Sprite health_sprite;
    public float max_length = 2.0f;

	// Use this for initialization
	void Start () {
        bar = gameObject.AddComponent<SpriteRenderer>();
        bar.sprite = health_sprite;
        transform.localScale = new Vector3(max_length, 0.3f, 1.0f);
    }
	
	public float health
    {
        set
        {
            //value passed in should be between 0.0 and 1.0
            float new_length = value * max_length;
            transform.position = new Vector3(max_length - value, 0, 0);
            transform.localScale = new Vector3(new_length, 0.3f, 1.0f);
        }
    }
}
