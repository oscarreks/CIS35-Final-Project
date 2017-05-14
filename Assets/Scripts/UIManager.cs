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
        maxWidth = manaRight.rectTransform.rect.width;
	}
	
    public void manaUpdate()
    {
        float ratio_0 = GameManager.instance.mana[0] / maxMana;
        float ratio_1 = (10 - GameManager.instance.mana[1]) / maxMana;

        manaLeft.rectTransform.sizeDelta = new Vector2(ratio_0 * maxWidth, manaLeft.rectTransform.rect.height);
        manaLeft.rectTransform.anchoredPosition = new Vector2(maxWidth/2 * (-1 + ratio_0), 0);
        
        manaRight.rectTransform.sizeDelta = new Vector2(ratio_1 * maxWidth, manaRight.rectTransform.rect.height);
        manaRight.rectTransform.anchoredPosition = new Vector2(maxWidth/2 * (1 - ratio_1), manaRight.rectTransform.anchoredPosition.y);
    }

    void Update()
    {
        manaUpdate();
    }
}
