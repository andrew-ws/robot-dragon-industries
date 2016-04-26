using UnityEngine;
using System.Collections;

public class Farmer : Enemy {

    public Vector3 velocity = Vector3.zero;

    public SpriteRenderer sr;
    public BoxCollider2D coll;

    public float chaseSpeed = 1.5f;
    public float wanderSpeed = 0.5f;

    public float switchDirectionOddsX = 15f;
    public float switchDirectionOddsY = 15f;

    public float forkSpeed = 6f;
    private bool thrown = false;
    public float throwThreshold;

    // Use this for initialization
    void Start () {
        aggroAdd = 3;
        this.transform.localScale = new Vector3(1f, 1f, 1f);
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/farmer");
        coll = gameObject.AddComponent<BoxCollider2D>();
        coll.size = new Vector2(1f, 1.5f);
        coll.offset = new Vector2(0f, -.3f);
        coll.isTrigger = true;
        float dRand =  Random.value;
        if (dRand * 100 < 25)
            velocity = (Vector2.up  + Vector2.left).normalized * wanderSpeed;
        else if (dRand * 100 < 50)
            velocity = (Vector2.up + Vector2.right).normalized * wanderSpeed;
        else if (dRand * 100 < 75)
            velocity = (Vector2.down + Vector2.left).normalized * wanderSpeed;
        else
            velocity = (Vector2.down + Vector2.right).normalized * wanderSpeed;

        throwThreshold = lm.boundLeft * 2 / 3;
        if (isAngry) makeAngry();
    }
	
	// Update is called once per frame
	void Update () {
        if (!isAngry)
        {
            if (Random.value * 100 < (switchDirectionOddsX * Time.deltaTime))
                velocity.x = -velocity.x;
            if (Random.value * 100 < (switchDirectionOddsY * Time.deltaTime))
                velocity.y = -velocity.y;
            if (transform.position.y >= lm.boundUp)
                velocity = Vector2.down * wanderSpeed;
            if (transform.position.y <= lm.boundDown)
                velocity = Vector2.up * wanderSpeed;
        }

		Vector2 diff = new Vector2();
		if (lm.player.hp > 0) {
			diff = lm.player.transform.position - this.transform.position;
		}
        if (isAngry) velocity = diff.normalized * chaseSpeed;
        if (isAngry && !stunned && !thrown && transform.position.x < 
            throwThreshold)
        {
            thrown = true;
            throwFork();
        } 
        if (stunned)
        {
            if (stunClock > stunTime)
            {
                stunned = false;
                makeAngry();
                stunClock = 0f;
            }
            else velocity = Vector2.zero;
            stunClock += Time.deltaTime;
        }

        this.transform.position += (velocity + lm.bgSpeed * Vector3.left)
            *Time.deltaTime;

        clock += Time.deltaTime;
        if (clock > timeAlive) Destroy(gameObject);

        // sorting order hack
        sr.sortingOrder = 10000 - (int)
            ((transform.position.y - (sr.bounds.size.y)) * 100);
    }

    private void throwFork()
    {
        GameObject go = new GameObject();
        go.transform.parent = lm.projectileFolder.transform;
        Projectile fork = go.AddComponent<Projectile>();
        fork.velocity = Vector3.right * forkSpeed;
        fork.transform.position = this.transform.position;
        fork.init(Vector3.right, forkSpeed, lm.bgSpeed * Vector3.left);
        fork.name = "Pitchfork";
        fork.setSprite("Sprites/pitchfork");
        fork.sr.flipX = true;
    }

    protected override void onHit()
    {
        // stun code
        velocity = Vector2.zero;
        // set sprites to stunt
        sr.color = Color.blue;
        stunned = true;
    }

    protected override void makeAngry()
    {
        isAngry = true;
        sr.color = Color.red;
        // change sprite
    }
}
