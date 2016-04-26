using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public Vector3 velocity = Vector3.zero;
    public float timeAlive = 20f;
    public float clock = 0f;

    protected BoxCollider2D coll;
    protected Rigidbody2D rb;
    public SpriteRenderer sr;

    void Start() {
        initComponents();
        coll.size = new Vector2(1.8f, 0.18f);
    }

    /*
        Physics note: projectiles have colliders and rigidbodies.
        The rigidbodies allow them to interact with objects they touch,
        and the colliders have isTrigger set to true so they don't
        have physics interactions with the bounding box or player.
    */
    protected void initComponents()
    {
        coll = gameObject.AddComponent<BoxCollider2D>();
        coll.isTrigger = true;

        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }
    
    // initVel is relative to the camera
    public void init(Vector3 direction, float speed, Vector3 initVel)
    {
        this.velocity = direction.normalized * speed + initVel;
    }
	
	// Update is called once per frame
	void Update ()
    {
        move();
        // sorting order hack
        sr.sortingOrder = 10000 + (int)
            ((transform.position.y - ( sr.bounds.size.y / 2 )) * 100);
    }

    protected void move()
    {

        this.transform.position += velocity * Time.deltaTime;

        if (clock > timeAlive) Destroy(this.gameObject);
        clock += Time.deltaTime;

        // TODO: make air resistance a thing?
    }

    public void setSprite(string spriteName)
    {
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>(spriteName);
    }

    public void OnTriggerEnter2D(Collider2D coll) {
        if (coll.gameObject.CompareTag("player"))
        {
            Player p = coll.gameObject.GetComponent<Player>();
            p.hurt();
            Destroy(gameObject);
        }
    }
}
