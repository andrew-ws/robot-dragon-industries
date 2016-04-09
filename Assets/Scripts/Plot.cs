using UnityEngine;
using System.Collections;

public class House : MonoBehaviour {

    private int type; // type of plot (0=empty, 1=building, 2=other)
    private float size; // size of plot in unity units
    private float speed; // rate a which it passes the player
    private PlotModel model; // model containing all sprites
    public LevelManager manager; // easy reference

    /*
    Note: Eventually I think it might be best to have the Plot class have several quads.
    One would be for the building if there is one, some for scenery perhpas, and then also
    one for targets such as mailboxes. For now I just have a quad for a building.
    */

    public void init(int type, LevelManager manager)
    {
        this.type = type;
        this.manager = manager;

        //defaults (not set by args)
        size = 6;
        //speed = LevelManager.getSpeed()     probably should be set by the level manager
        speed = 4;

        var buildingObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        MeshCollider colid = buildingObject.GetComponent<MeshCollider>();
        DestroyImmediate(colid);
        BoxCollider2D box = buildingObject.AddComponent<BoxCollider2D>();
        Rigidbody2D rig = buildingObject.AddComponent<Rigidbody2D>(); //needed?
        buildingObject.SetActive(true);

        rig.isKinematic = true;
        box.isTrigger = true;
        box.size = new Vector2(6,6);

        model = buildingObject.AddComponent<PlotModel>();
        //model.init(x, y, direction, m.speed, this);

    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;

        //move game object to the left
        x = speed * Time.deltaTime;
        transform.position = new Vector3(x, y, z);
	}
}
