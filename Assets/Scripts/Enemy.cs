using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour {

    protected int aggroAdd = 1;

    protected bool isAngry = false;
    protected bool stunned = false;
    protected float stunClock = 0f;
    protected float stunTime = 2f;

    public LevelManager lm;

	public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("mailbox")) return;
        GameObject other = coll.gameObject;
        if (other.CompareTag("player"))
        {
            onHit();
            other.GetComponent<Player>().hurt();
        }
        else Destroy(other);
        lm.hitAggro(aggroAdd);
        onHit();
        Destroy(this.gameObject);
    }

    public void init(LevelManager lm, bool isAngry)
    {
        this.lm = lm;
        if (isAngry) makeAngry();
    }

    protected abstract void onHit();

    protected abstract void makeAngry();
}
