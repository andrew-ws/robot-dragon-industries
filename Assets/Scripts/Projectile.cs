using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public Vector3 velocity = Vector3.zero;
    public float timeAlive = 1000f;
    private float clock = 0f;

    // Use this for initialization
    void Start() {
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
    }

    protected void move()
    {

        this.transform.position += velocity * Time.deltaTime;

        if (clock > timeAlive) Destroy(this.gameObject);
        clock += Time.deltaTime;

        // TODO: make air resistance a thing?
    }
}
