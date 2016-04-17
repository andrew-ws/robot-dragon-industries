﻿using UnityEngine;
using System.Collections;

public class Cow : Enemy {

    public Vector3 velocity = new Vector3(-1, 0, 0);

    public SpriteRenderer sr;
    public BoxCollider2D coll;

    public float cowVision = 0.2f;
    public bool charging = false;

    public float wanderSpeed = 1f;
    public float switchDirectionOdds = 60f;
    public float angrySpeedBoost = 0.5f;

    private static float chargeGate = 1f;
    private static float chargeSpeed = 4.5f;

	// Use this for initialization
	void Start () {
        this.transform.localScale = new Vector3(0.18f, 0.18f, 1f);
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/cow");
        coll = gameObject.AddComponent<BoxCollider2D>();
        coll.size = new Vector2(7.5f, 4.2f);
        coll.offset = new Vector2(1.67f, 0.9f);
        coll.isTrigger = true;
        if (Random.value * 100 < 50) velocity = Vector2.up * wanderSpeed;
        else velocity = Vector2.down * wanderSpeed;
    }
	
	// Update is called once per frame
	void Update () {
        if (!charging)
        {
            if (Random.value * 100 < (switchDirectionOdds * Time.deltaTime))
                velocity.y = -velocity.y;
            if (transform.position.y >= lm.boundUp)
            {
                velocity = Vector2.down * wanderSpeed;
            }
            if (transform.position.y <= lm.boundDown)
            {
                velocity = Vector2.up * wanderSpeed;
            }
            if (isAngry) velocity.x = -angrySpeedBoost;
        }
        if (stunned)
        {
            if (stunClock > stunTime) {
                stunned = false;
                makeAngry();
                if (Random.value * 100 < 50)
                    velocity = Vector2.up * wanderSpeed;
                else velocity = Vector2.down * wanderSpeed;
            }
            else velocity = Vector2.zero;
            stunClock += Time.deltaTime;
        }

        this.transform.position += (velocity + lm.bgSpeed * Vector3.left)
            * Time.deltaTime;

        if (isAngry && !charging && Mathf.Abs(transform.position.y -
            lm.player.transform.position.y) < cowVision &&
            transform.position.x < (LevelManager.rdWidth/2 - chargeGate))
        {
            velocity = new Vector3(-chargeSpeed, 0, 0);
            charging = true;
        }
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
        sr.color = Color.red;
        // change sprite
    }

}
