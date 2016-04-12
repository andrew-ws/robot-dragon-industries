using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    protected int aggroAdd = 1;

    protected LevelManager lm;

	public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("mailbox")) return;
        GameObject other = coll.gameObject;
        if (other.CompareTag("player")) other.GetComponent<Player>().hurt();
        else Destroy(other);
        lm.hitAggro(aggroAdd);
        Destroy(this.gameObject);
    }
}
