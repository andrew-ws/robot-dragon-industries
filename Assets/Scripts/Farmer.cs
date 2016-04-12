using UnityEngine;
using System.Collections;

public class Farmer : Enemy {

    public Vector3 velocity = Vector3.zero;

    public SpriteRenderer sr;
    public BoxCollider2D coll;

    protected int aggroAdd = 3;

    // Use this for initialization
    void Start () {
        lm = GameObject.Find("GameObject").GetComponent<GameManager>().lm;
        this.transform.localScale = new Vector3(0.3f, 0.3f, 1f);
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/farmer");
        coll = gameObject.AddComponent<BoxCollider2D>();
        coll.isTrigger = true;
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.position += (velocity + lm.bgSpeed * Vector3.left)
            *Time.deltaTime;
        transform.position += 2 * Random.value * Time.deltaTime * Vector3.up;
        transform.position += 2 * Random.value * Time.deltaTime * Vector3.down;
    }
}
