using UnityEngine;
using System.Collections;

public class Newspaper : Projectile
{

    void Start() {
        gameObject.tag = "paper";
        initComponents();
        transform.localScale = new Vector3(0.15f, 0.15f, 1);
        sr.sprite = Resources.Load<Sprite>("Sprites/paper");
        coll.radius = 0.18f;
    }

    void Update()
    {
        move();
        transform.eulerAngles = new Vector3(0, 0, 360 * clock * 2);
    }

}