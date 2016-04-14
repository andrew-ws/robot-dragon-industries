using UnityEngine;
using System.Collections;

public class Scenery : MonoBehaviour {

    private string image;
    private float speed;
    private Material mat;
    private LevelManager manager;

    public void init(string image, float speed, LevelManager manager)
    {
        this.image = image;
        this.speed = speed;
        this.manager = manager;

        mat = gameObject.GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Sprites/Default");
        mat.color = new Color(1, 1, 1);
        mat.mainTexture = Resources.Load<Texture2D>("Sprites/" + image);
    }
	
	// Update is called once per frame
	void Update () {
        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;

        //move game object to the left
        x -= speed * Time.deltaTime;
        transform.position = new Vector3(x, y, z);

        if (transform.position.x < manager.boundLeft - 6)
        {
            Destroy(gameObject);
        }
    }
}
