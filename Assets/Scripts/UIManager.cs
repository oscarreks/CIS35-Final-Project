using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public Image manaLeft;
    public Image manaRight;
    public float maxWidth;
    public float maxMana;

    private UIManager instance;

    void Awake()
    {
        if (instance == null) //if this object has not been instantiated yet, assign it the global status
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start () {
        maxMana = GameManager.instance.maxMana;
        maxWidth = manaLeft.rectTransform.rect.width;
	}
	
    public void manaUpdate()
    {
        float ratio_0 = GameManager.instance.mana[0] / maxMana;
        float ratio_1 = GameManager.instance.mana[1] / maxMana;
        print("ratio_0 = " + ratio_0);
        manaLeft.rectTransform.sizeDelta = new Vector2(ratio_0 * maxWidth, manaLeft.rectTransform.rect.height);
        //manaLeft.rectTransform.localScale = new Vector3(ratio_0, 1, 1);
    }

    void Update()
    {
        manaUpdate();
    }
}
