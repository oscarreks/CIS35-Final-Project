using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour {

    public float damage;
    public float radius;
    public float force;
    public float duration = 0.625f;

    public virtual void startDamage(TEAM t, Vector2 pos)
    {
        List<GameObject> units;

        if (t == TEAM.GREEN)
        {
            units = GameManager.instance.team1;
        }
        else
        {
            units = GameManager.instance.team2;
        }

        //Start animation coroutine here

        Vector2 center = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        foreach (GameObject unit in units)
        {
            if (Vector2.Distance(center, unit.transform.position) < radius)
            {
                unit.BroadcastMessage("addHealth", -damage);
                Vector2 knockback = (Vector2)unit.transform.position - center;
                knockback = knockback.normalized * force;
                unit.GetComponent<Rigidbody2D>().AddForce(knockback);
            }
        }
    }

    private void Start()
    {
        StartCoroutine(animate());
    }

    private IEnumerator animate()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}
