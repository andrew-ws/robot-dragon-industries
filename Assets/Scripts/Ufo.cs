using UnityEngine;
using System.Collections;

public class Ufo : Enemy {

    public Vector3 velocity;

    public SpriteRenderer sr;

    public float ufoSpeed = 2f;
    public float laserClock = 0f;
    public float laserTime = 3f;
    public float laserSpeed = 3f;

    // Use this for initialization
    void Start()
    {
        velocity = Vector3.right * ufoSpeed;
        aggroAdd = 0;
        this.transform.localScale = new Vector3(1f, 1f, 1f);
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/farmer");
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < lm.boundLeft)
            velocity = ufoSpeed * Vector3.right;
        if (transform.position.x > lm.boundRight)
            velocity = ufoSpeed * Vector3.left;

        this.transform.position += velocity * Time.deltaTime;

        clock += Time.deltaTime;
        if (clock > timeAlive) Destroy(gameObject);

        laserClock += Time.deltaTime;
        while (laserClock > laserTime)
        {
            laserClock -= laserTime;
            GameObject go = new GameObject();
            go.transform.parent = lm.projectileFolder.transform;
            Projectile laser = go.AddComponent<Projectile>();
            laser.velocity = Vector3.down * laserSpeed;
            laser.transform.position = this.transform.position;
            laser.init(Vector3.down, laserSpeed, lm.bgSpeed * Vector3.left);
            laser.name = "Laser";
            laser.setSprite("Sprites/pitchfork");

            lm.manager.PlayEffect(lm.manager.throwPaper);
        }

        // sorting order hack
        sr.sortingOrder = 10000 - (int)
            ((transform.position.y - (sr.bounds.size.y)) * 100);
    }

    protected override void onHit()
    {
        // this doesn't happen
    }

    protected override void makeAngry()
    {
        isAngry = true;
        sr.color = Color.white;
        // TODO: replace anger sprite
        sr.sprite = Resources.Load<Sprite>("Sprites/UFOMad");
    }
}
