using UnityEngine;
using System.Collections;

public class Cow : Enemy {

    public Vector3 velocity = new Vector3(-1, 0, 0);

    public bool isMad = false;

    public SpriteRenderer sr;
    public BoxCollider2D coll;

	// Use this for initialization
	void Start () {
        if (isMad) velocity *= 2;
        lm = GameObject.Find("GameObject").GetComponent<GameManager>().lm;
        this.transform.localScale = new Vector3(0.18f, 0.18f, 1f);
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/cow");
        if (isMad) sr.color = Color.red;
        coll = gameObject.AddComponent<BoxCollider2D>();
        coll.isTrigger = true;
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.position += (velocity + lm.bgSpeed * Vector3.left)
            * Time.deltaTime;
	}
}
