using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    
    public float speed = 3f;
    public float throwSpeed = 5f;
    private Vector3 deltaPos;
    public int hp = 3;

    private float cooldownClock;

    private SpriteRenderer sr;

	// Use this for initialization
	void Start () {
        gameObject.tag = "player";

        gameObject.AddComponent<SpriteRenderer>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/bike");
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

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            shoot(Vector2.left);
        if (Input.GetKeyDown(KeyCode.RightArrow))
            shoot(Vector2.right);
        if (Input.GetKeyDown(KeyCode.UpArrow))
            shoot(Vector2.up);
    }

    void shoot(Vector2 dir) {
        if (cooldownClock > 0) return;
        Newspaper paper = (new GameObject()).AddComponent<Newspaper>();
        paper.transform.position = this.transform.position;
        paper.init(dir, throwSpeed, deltaPos);
        cooldownClock = 0.5f;
    }

    public void hurt()
    {
        hp--;
        if (hp == 0) Destroy(this.gameObject);
    }
}