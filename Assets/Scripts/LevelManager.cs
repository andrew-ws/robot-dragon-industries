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

    private float backgroundHt; // height of background based on width
    private float backgroundy; // offset so background's bottom edge is the same as the camera's

    private float plotClock;
    private float sceneryClock;

    public float cowOddsBase = 20f;
    public float cowOddsPerAggro = 2f;
    public float farmerOddsBase = 15f;
    public float farmerOddsPerAggro = 2f;

    public Player player;
	public GameManager manager;
	// I need our main GameManager to be passed along into MusicManager to access the audio
	// that is attached to it

    public int aggro = 1;
    public int capAggro = 30;
    public int baseAggro = 1;
    //public bool readyForDrop = false;
    public bool dropped = false;

    public float aggroLossTimer = 0f;
    public const float aggroLossTime = 3f;

    public int thresholdAggro = 15;
    public float thresholdTime = 3f;
    public float thresholdClock = 0f;

    public float bgSpeed = 2f;

    private GameObject sky; // back-most layer of the background (probably only temporary)
    private GameObject street;

    private GameObject plotFolder;
    private GameObject sceneryFolder;
    
    public int totalMoney = 0;

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
        plotClock = 6;
        sceneryClock = 1;

        player = (new GameObject()).AddComponent<Player>();
        player.lm = this;
        player.gameObject.name = "Player";
        player.gameObject.transform.localPosition = Vector3.zero;

        backgroundHt = rdWidth * (7f / 8f);
        backgroundy = (backgroundHt - (cam.orthographicSize * 2)) / 2 + camy;

        sky = makeBackground("skyDay1", 3);
        street = makeBackground("street", 2);

        sceneryFolder = new GameObject();
        sceneryFolder.name = "Scenery";
        plotFolder = new GameObject();
        plotFolder.name = "Plots";
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

        plotClock += Time.deltaTime;
        sceneryClock += Time.deltaTime;
        System.Random rnd = new System.Random();

        if (plotClock >= 3) // should happen every 6 secs, we need to scale this to how fast everything is moving
        {
            plotClock = 0 + Time.deltaTime;
            spawnPlot(rnd.Next(1, 3));
        }
        if (sceneryClock >= (3f/4f))
        {
            sceneryClock = 0 + Time.deltaTime;
            spawnLine();
            spawnSidewalk();
        }
        manageAggro();
        spawnCows();
        spawnFarmers();

    }
    /*
    Note: the spawners below are temporary as they are quite redundant.
    I would like to be able to have the spawners themselves take care of
    the frequency at which certain pieces of scenery spawn and not the
    Update() method of the LevelManager. This requires an individual clock
    for each one. NM
    */

    //used to spawn plots at regular intervals
    private void spawnPlot(int type)
    {
        Plot plot = (new GameObject()).AddComponent<Plot>();
        plot.transform.position = new Vector3(rdWidth/2f+3, rdHeight);
        plot.transform.parent = plotFolder.transform;
        plot.init(type, this);
    }

    private void spawnSidewalk()
    {
        var sceneryObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Scenery tile = sceneryObject.AddComponent<Scenery>();
        sceneryObject.transform.parent = sceneryFolder.transform;
        sceneryObject.name = "Sidewalk";
        tile.transform.localScale = new Vector3(rdWidth / 8f, 1, 0);
        tile.transform.position = new Vector3(rdWidth/2f+1, 1.5f, 0);
        tile.init("sidewalkSuburban", bgSpeed, this);
    }

    private void spawnLine()
    {
        var sceneryObject = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Scenery line = sceneryObject.AddComponent<Scenery>();
        sceneryObject.transform.parent = sceneryFolder.transform;
        sceneryObject.name = "Line";
        line.transform.localScale = new Vector3(1, 1, 0);
        line.transform.position = new Vector3(rdWidth/2f+1, -1, 0);
        line.init("streetLine", bgSpeed, this);
    }

    private GameObject makeBackground(string image, int layer)
    {
        GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Quad);
        Material mat = obj.GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Sprites/Default");
        mat.color = new Color(1, 1, 1);
        mat.mainTexture = Resources.Load<Texture2D>("Sprites/" + image);
        obj.transform.localScale = new Vector3(rdWidth, backgroundHt, 0);
        obj.transform.position = new Vector3(0, backgroundy, layer);

        if (layer == 2) { obj.name = "Street"; }
        if (layer == 3) { obj.name = "Sky"; }

        return obj;
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
			this.drop ();
        }
		if (aggro < thresholdAggro && manager.dropped == true) {
			this.undrop ();
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
        GUI.Label(new Rect(Screen.width - 110, Screen.height - 50, 110, 50), "Money: " + totalMoney);
		GUI.Label(new Rect(Screen.width - 110, Screen.height - 150, 110, 50), "Aggro: " + aggro);
        GUI.Label(new Rect(0, 0, 110, 50), "Money per paper: " + (aggro * 50));
    }

    public void drop()
    {
        dropped = true;
    }

	public void undrop() {
		dropped = false;
	}

    private GameObject cow = Resources.Load<GameObject>("Prefabs/Cow");
    private GameObject farmer = Resources.Load<GameObject>("Prefabs/Farmer");
    private GameObject madCow = Resources.Load<GameObject>("Prefabs/MadCow");
    private GameObject angryFarmer = Resources.Load<GameObject>("Prefabs/AngryFarmer");
}
