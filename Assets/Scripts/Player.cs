using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    
    public float speed = 3f;
    public float throwSpeed = 5f;
    private Vector3 deltaPos;
    public int hp = 3;
    public LevelManager lm = null;

    private Vector2 collOffset = new Vector2(0f, -0.42f);
    private Vector2 collSize = new Vector2(0.9f, 0.065f);

    private float cooldownClock;

    private SpriteRenderer sr;
    private BoxCollider2D coll;
    private Rigidbody2D rb;

	/*
        Physics: player has a non-trigger collider and a rigidbody
        so it can interact with the bounding box and the enemies.
    */
	void Start () {
        gameObject.tag = "player";
        transform.localScale = new Vector3(2f, 2f, 1f);

        gameObject.AddComponent<SpriteRenderer>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/bike");

        coll = gameObject.AddComponent<BoxCollider2D>();
        coll.offset = collOffset;
        coll.size = collSize;
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }
	
	// Update is called once per frame
	void Update () {
        cooldownClock -= Time.deltaTime;

        deltaPos = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            deltaPos.y += speed*Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
            deltaPos.x -= speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S))
            deltaPos.y -= speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.D))
            deltaPos.x += speed * Time.deltaTime;

        // TODO: normalized diagonal speed instead?
        transform.position += deltaPos;

        // stay in bounding box
        if (transform.position.x < lm.boundLeft)
            transform.position = new Vector2(lm.boundLeft, transform.position.y);
        if (transform.position.x > lm.boundRight)
            transform.position = new Vector2(lm.boundRight, transform.position.y);
        if (transform.position.y < lm.boundDown)
            transform.position = new Vector2(transform.position.x, lm.boundDown);
        if (transform.position.y > lm.boundUp)
            transform.position = new Vector2(transform.position.x, lm.boundUp);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            shoot(Vector2.left);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            shoot(Vector2.right);
        if (Input.GetKeyDown(KeyCode.UpArrow))
            shoot(Vector2.up);
        if (Input.GetKeyDown(KeyCode.DownArrow))
            shoot(Vector2.down);

    }

    void shoot(Vector2 dir) {
        if (cooldownClock > 0) return;
        Newspaper paper = (new GameObject()).AddComponent<Newspaper>();
        paper.transform.position = this.transform.position;
        paper.name = "Paper";
        paper.init(dir, throwSpeed, deltaPos/Time.deltaTime/4);
        cooldownClock = 0.5f;
    }

    public void hurt()
    {
        hp--;
        if (hp == 0) Destroy(this.gameObject);
    }
}
