using UnityEngine;
using System.Collections;

public class Plot : MonoBehaviour {

    private int type; // type of plot (0=empty, 1=building, 2=other)
    private float size; // size of plot in unity units
    private float speed; // rate a which it passes the player
    private PlotModel model; // model containing all sprites
    public LevelManager manager; // easy reference?

    public void init(int type, LevelManager manager)
    {
        this.type = type;
        this.manager = manager;

        //defaults (not set by args)
        size = 6;
        //speed = LevelManager.getSpeed()     probably should be set by the level manager
        speed = 1;

        if (type == 1) // plot with a house
        {
            GameObject house = makeQuad(0, false);
            house.transform.localPosition = new Vector3(0, 0, 1);
            house.transform.localScale = new Vector3(5, 5, 0);

            GameObject mailbox = makeQuad(1, true);
            mailbox.transform.localPosition = new Vector3(-1, -2.5f, 0);
            mailbox.GetComponent<BoxCollider2D>().offset = new Vector2(0, .25f);
            mailbox.GetComponent<BoxCollider2D>().size = new Vector2(.5f, .5f);
            gameObject.name = "Plot: House";
        }
        else if (type == 2) // plot without a house
        {
            GameObject scenery = makeQuad(2, false);
            scenery.transform.localPosition = Vector3.zero;
            scenery.transform.localScale = new Vector3(6,6,0);
            gameObject.name = "Plot: Scenery";
        }

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
        x -= speed * Time.deltaTime;
        transform.position = new Vector3(x, y, z);

        if (transform.position.x < manager.boundLeft - 6)
        {
            Destroy(gameObject);
        }
	}

    private GameObject makeQuad(int type, bool hasCollider) // type here refers to which quad
    {
        //quad for scenery (e.g. fences, fields, corn on lvl 1)
        var sceneryObject = GameObject.CreatePrimitive(PrimitiveType.Quad);

        if (hasCollider)
        {
            MeshCollider colid2 = sceneryObject.GetComponent<MeshCollider>();
            DestroyImmediate(colid2);
            BoxCollider2D box2 = sceneryObject.AddComponent<BoxCollider2D>();
            Rigidbody2D rig2 = sceneryObject.AddComponent<Rigidbody2D>(); //needed?
            sceneryObject.SetActive(true);

            rig2.isKinematic = true;
            box2.isTrigger = true;
            box2.size = new Vector2(1, 1);
        }
        model = sceneryObject.AddComponent<PlotModel>();
        model.init(type, this);
        model.tag = "mailbox";
        return sceneryObject;
    }
}
