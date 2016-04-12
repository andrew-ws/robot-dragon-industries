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

    public float cowOddsBase = 30f;
    public float cowOddsPerAggro = 5f;
    public float farmerOddsBase = 15f;
    public float farmerOddsPerAggro = 5f;

    public Player player;
	public GameManager manager;
	// I need our main GameManager to be passed along into MusicManager to access the audio
	// that is attached to it

    public int aggro = 1;
    public int capAggro = 30;
    public bool readyForDrop = false;
    public bool dropped = false;

    public float bgSpeed = 1f;

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

        
        farmer = Resources.Load<GameObject>("Prefabs/Farmer");
        madCow = Resources.Load<GameObject>("Prefabs/MadCow");
        angryFarmer = Resources.Load<GameObject>("Prefabs/AngryFarmer");

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
		spawnCows();
		spawnFarmers();
	}

    public void hitAggro(int aggroAdd)
    {
        aggro += aggroAdd;
        if (aggro > capAggro) aggro = capAggro;
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

    public void drop()
    {
        print("Stage 2! Stuff is weird and frantic!");
    }

    private GameObject cow = Resources.Load<GameObject>("Prefabs/Cow");
    private GameObject farmer;
    private GameObject madCow;
    private GameObject angryFarmer;
}
