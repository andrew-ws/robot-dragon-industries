using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour {

    protected int aggroAdd = 1;

    protected bool isAngry = false;
    protected bool stunned = false;
    protected float stunClock = 0f;
    protected float stunTime = 2f;

    protected float timeAlive = 40f;
    protected float clock = 0f;

    // for spawning
    public static float minTimeBase;
    public static float minTimeAggro;
    public static float spreadTimeBase;
    public static float spreadTimeAggro;

    public static float spawnClock = 0;
    public static float spawnNext = minTimeBase;

    public LevelManager lm;

	public void OnTriggerEnter2D(Collider2D coll)
    {
        if (stunned) return;
        if (coll.gameObject.CompareTag("mailbox")) return;
        GameObject other = coll.gameObject;
        if (other.CompareTag("player"))
        {
            if (!stunned) lm.hitAggro(aggroAdd);
            onHit();
            other.GetComponent<Player>().hurt();
        }
        else if (other.CompareTag("paper"))
        {
            Destroy(other);
            if (!stunned) lm.hitAggro(aggroAdd);
            onHit();
        }
    }

    public void init(LevelManager lm, bool isAngry)
    {
        this.lm = lm;
        this.isAngry = isAngry;
    }

    protected abstract void onHit();

    protected abstract void makeAngry();
}
