using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {

    public float clock = 0f;
    public int pickupType; // [0=bundle, 1=?...]
    public bool open = true; // whether the mailbox is open
    protected BoxCollider2D coll;
    protected Rigidbody2D rb;
    protected SpriteRenderer sr;

    // initVel is relative to the camera
    public void init(int pickupType)
    {
        this.pickupType = pickupType;

        coll = gameObject.AddComponent<BoxCollider2D>();
        coll.isTrigger = true;

        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        sr = gameObject.AddComponent<SpriteRenderer>();

        //transform.localScale = new Vector3(1, 1, 1);
        //transform.localPosition = new Vector3(Random.Range(-2.5f, 2.5f), -2.5f, 1);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
