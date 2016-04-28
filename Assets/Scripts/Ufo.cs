using UnityEngine;
using System.Collections;

public class Ufo : Enemy {

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

    public new static float minTimeBase = 4f;
    public new static float minTimeAggro = -0.12f;
    public new static float spreadTimeBase = 1f;
    public new static float spreadTimeAggro = -0.05f;

    public new static float spawnClock = 0;
    public new static float spawnNext = minTimeBase;

    // Use this for initialization
    void Start()
    {
        aggroAdd = 3;
        this.transform.localScale = new Vector3(1f, 1f, 1f);
        sr = gameObject.AddComponent<SpriteRenderer>();
        // TODO: change the sprite out
        sr.sprite = Resources.Load<Sprite>("Sprites/farmer");
        coll = gameObject.AddComponent<BoxCollider2D>();
        coll.size = new Vector2(1f, 1.5f);
        coll.offset = new Vector2(0f, -.3f);
        coll.isTrigger = true;
        float dRand = Random.value;
        if (dRand * 100 < 25)
            velocity = (Vector2.up + Vector2.left).normalized * wanderSpeed;
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
    void Update()
    {
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
        if (lm.player.hp > 0)
        {
            diff = lm.player.transform.position - this.transform.position;
        }
        if (isAngry) velocity = diff.normalized * chaseSpeed;

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
            * Time.deltaTime;

        clock += Time.deltaTime;
        if (clock > timeAlive) Destroy(gameObject);

        // sorting order hack
        sr.sortingOrder = 10000 - (int)
            ((transform.position.y - (sr.bounds.size.y)) * 100);
    }

    protected override void onHit()
    {
        // stun code
        velocity = Vector2.zero;
        // set sprites to stun
        sr.color = Color.blue;
        stunned = true;
    }

    protected override void makeAngry()
    {
        isAngry = true;
        sr.color = Color.white;
        // TODO: replace anger sprite
        sr.sprite = Resources.Load<Sprite>("Sprites/farmerMad");
    }
}
