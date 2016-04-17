using UnityEngine;
using System.Collections;

public class Farmer : Enemy {

    public Vector3 velocity = Vector3.zero;

    public SpriteRenderer sr;
    public BoxCollider2D coll;

    public float chaseSpeed = 2f;
    public float wanderSpeed = 0.5f;

    public float switchDirectionOddsX = 15f;
    public float switchDirectionOddsY = 15f ;

    // Use this for initialization
    void Start () {
        aggroAdd = 3;
        this.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/farmer");
        coll = gameObject.AddComponent<BoxCollider2D>();
        coll.size = new Vector2(1.2f, 3f);
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

        Vector2 diff = lm.player.transform.position - this.transform.position;
        if (isAngry) velocity = diff.normalized * chaseSpeed;
        if (stunned)
        {
            if (stunClock > stunTime)
            {
                stunned = false;
                if (!isAngry) makeAngry();
            }
            else velocity = Vector2.zero;
            stunClock += Time.deltaTime;
        }

        this.transform.position += (velocity + lm.bgSpeed * Vector3.left)
            *Time.deltaTime;
    }

    protected override void onHit()
    {
        // stun code
        velocity = Vector2.zero;
        // set sprites to stun
        stunned = true;
        makeAngry();
    }

    protected override void makeAngry()
    {
        isAngry = true;
        // change sprite
    }
}
