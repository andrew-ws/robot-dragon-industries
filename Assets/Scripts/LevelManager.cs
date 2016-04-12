using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public const float rdWidth = 16f;
    public const float rdHeight = 4f;
    public const float rdMargin = 0.5f;
    public const float rdPadTop = 0.3f;
    public const float rdPadSide = 1f;
    public const float rdPadBottom = 0.3f;

    public float boundUp, boundDown, boundLeft, boundRight;

    public Player player;
	public GameManager manager;
	// I need our main GameManager to be passed along into MusicManager to access the audio
	// that is attached to it

    public int aggro = 1;

    private GameObject sky; // back-most layer of the background (probably only temporary)

	// Use this for initialization
	void Start () {
        this.transform.position = Vector3.zero;

        // set up camera
        Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        cam.orthographicSize = (rdWidth / cam.aspect) / 2;
        float camy = cam.orthographicSize - rdHeight / 2 - rdMargin;
        cam.transform.position = new Vector3(0, camy, -10);

        /* calculate bounds
            This is done in this seemingly redundant way so we can tweak
            them later if we need to.
        */
        boundUp = (rdHeight / 2) - rdPadTop;
        boundDown = (-rdHeight / 2) + rdPadBottom;
        boundLeft = (-rdWidth / 2) + rdPadSide;
        boundRight = (rdWidth / 2) - rdPadSide;

        player = (new GameObject()).AddComponent<Player>();
        player.lm = this;
        player.gameObject.name = "Player";
        player.gameObject.transform.localPosition = Vector3.zero;

        // create sky background
        sky = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Material mat = sky.GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Sprites/Default");
        mat.color = new Color(1, 1, 1);
        mat.mainTexture = Resources.Load<Texture2D>("Sprites/skyDay1");
        sky.transform.position = new Vector3(0, camy, 0);
        sky.transform.localScale = new Vector3(rdWidth,rdHeight*3, 0);
	}

    /*
        About the level construction: the bounding box is a bunch
        of box colliders (no rigidbody). The player should have the only
        other collider without isTrigger set to true, so it's the only
        one that has physics interactions with the bounding box.
        We may want to redo the bounding box with our own code later, but
        it's a decent strategy for now.
    */
	public void init (int whichLevel, GameManager manager)
    {
		this.manager = manager;
        // setup bounding box
        // setup spawners
        // setup camera?
    }

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			print ("space");
			manager.drop ();
		}

	}


}
