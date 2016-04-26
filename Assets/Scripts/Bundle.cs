using UnityEngine;
using System.Collections;

public class Bundle : Pickup {

    LevelManager m;
	private int paperAdd = 5;

	// Use this for initialization
	public void init(LevelManager m) {

        base.init(0);

        this.m = m;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/bundle");
        gameObject.name = "Bundle";

        transform.localScale = new Vector3(1, 1, 0);
        transform.position = new Vector3(LevelManager.rdWidth / 2f + 1, 1.5f, 0);

    }
	
	// Update is called once per frame
	void Update () {

        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;

        //move game object to the left
        x -= m.bgSpeed * Time.deltaTime;
        transform.position = new Vector3(x, y, z);

        if (transform.position.x < m.boundLeft - 6)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (open)
        {
            if (!other.gameObject.CompareTag("player")) return;
            Destroy(gameObject);
			if (m.player.papers + paperAdd <= m.player.maxpapers) {
				m.player.papers += paperAdd;
			} else {
				m.player.papers = m.player.maxpapers;
			}
        }
    }
}
