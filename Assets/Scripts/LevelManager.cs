using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour {

    public const float rdWidth = 16f;
    public const float rdHeight = 5f;
    public const float rdMargin = 0.5f;
    public const float rdPadTop = 0.3f;
    public const float rdPadSide = 1f;
    public const float rdPadBottom = 0.3f;
    public const float spawnPtPad = 1.5f;

    public float boundUp, boundDown, boundLeft, boundRight;


    private float clock;

    public float cowOddsBase = 20f;
    public float cowOddsPerAggro = 3f;
    public float farmerOddsBase = 15f;
    public float farmerOddsPerAggro = 3f;

    public Player player;
	public GameManager manager;
	// I need our main GameManager to be passed along into MusicManager to access the audio
	// that is attached to it

    public int aggro = 1;
    public int capAggro = 30;
    public int baseAggro = 1;
    public bool readyForDrop = false;
    public bool dropped = false;

    public float aggroLossTimer = 0f;
    public const float aggroLossTime = 3f;

    public int thresholdAggro = 15;
    public float thresholdTime = 3f;
    public float thresholdClock = 0f;

    public float bgSpeed = 1f;

    private GameObject sky; // back-most layer of the background (probably only temporary)
    private GameObject street;

    public int deliveries; // # of newspapers delivered

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

        clock = 6;
        deliveries = 0;

        player = (new GameObject()).AddComponent<Player>();
        player.lm = this;
        player.gameObject.name = "Player";
        player.gameObject.transform.localPosition = Vector3.zero;

        // create sky background (temporary)
        sky = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Material mat = sky.GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Sprites/Default");
        mat.color = new Color(1, 1, 1);
        mat.mainTexture = Resources.Load<Texture2D>("Sprites/skyDay1");
        sky.transform.position = new Vector3(0, camy, 3);
        sky.transform.localScale = new Vector3(rdWidth,rdHeight*3, 0);
        sky.name = "Sky";

        // create street (temporary, currently doesn't move)
        street = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Material mat2 = street.GetComponent<Renderer>().material;
        mat2.shader = Shader.Find("Sprites/Default");
        mat2.color = new Color(1, 1, 1);
        mat2.mainTexture = Resources.Load<Texture2D>("Sprites/streetRural");
        street.transform.position = new Vector3(0, camy+2, 2);
        street.transform.localScale = new Vector3(rdWidth, rdHeight*3, 0);
        street.name = "Street";
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

        clock += Time.deltaTime;
        System.Random rnd = new System.Random();

        if (Input.GetKeyDown (KeyCode.Space)) {
			print ("space");
			manager.drop ();
		}

        if (clock >= 6) // should happen every 6 secs, we need to scale this to how fast everything is moving
        {
            clock = 0;
            spawnPlot(rnd.Next(1, 3));
        }
        
        manageAggro();
        spawnCows();
        spawnFarmers();

    }

    //used to spawn plots at regular intervals
    private void spawnPlot(int type)
    {
        Plot plot = (new GameObject()).AddComponent<Plot>();
        plot.transform.position = new Vector3(rdWidth, rdHeight);
        plot.init(type, this);
    }

    public void hitAggro(int aggroAdd)
    {
        aggro += aggroAdd;
        if (aggro > capAggro) aggro = capAggro;
    }

    private void manageAggro()
    {
        if (aggro >= thresholdAggro)
        {
            thresholdClock += Time.deltaTime;
        }
        if (thresholdClock > thresholdTime)
        {
            readyForDrop = true;
        }
        aggroLossTimer += Time.deltaTime;
        if (aggro < baseAggro) aggro = baseAggro;
        if (aggro > baseAggro && aggroLossTimer > aggroLossTime)
            aggro--;
        aggroLossTimer %= aggroLossTime;
    }

    private void spawnCows() {
        float place = Random.value * rdHeight - rdHeight / 2;
        Vector3 spawnPt = new Vector3(spawnPtPad + (rdWidth / 2), place, 0);
        float chance = cowOddsBase + aggro * cowOddsPerAggro;
        if (chance * Time.deltaTime > Random.value * 100)
        {
            if (dropped)
                Instantiate(madCow, spawnPt, Quaternion.identity);
            else
                Instantiate(cow, spawnPt, Quaternion.identity);
        }
    }

    private void spawnFarmers() {
        float place = Random.value * rdHeight - rdHeight / 2;
        Vector3 spawnPt = new Vector3(spawnPtPad + (rdWidth / 2), place, 0);
        float chance = farmerOddsBase + aggro * farmerOddsPerAggro;
        if (chance * Time.deltaTime > Random.value * 100)
        {
            if (dropped)
                Instantiate(angryFarmer, spawnPt, Quaternion.identity);
            else
                Instantiate(farmer, spawnPt, Quaternion.identity);
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width - 110, Screen.height - 50, 110, 50), "Newspapers Delivered: " + deliveries);
    }

    public void drop()
    {
        dropped = true;
        print("Stage 2! Stuff is weird and frantic!");
    }

    private GameObject cow = Resources.Load<GameObject>("Prefabs/Cow");
    private GameObject farmer = Resources.Load<GameObject>("Prefabs/Farmer");
    private GameObject madCow = Resources.Load<GameObject>("Prefabs/MadCow");
    private GameObject angryFarmer = Resources.Load<GameObject>("Prefabs/AngryFarmer");
}
