using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    public int level;
    public string levelName;

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

    public float cowOddsBase = 14f;
    public float cowOddsPerAggro = 1.5f;
    public float farmerOddsBase = 10f;
    public float farmerOddsPerAggro = 1.5f;
	public int plotCap = 20;
	public int numPlots = 0;
	public int numSide = 0;

    public Player player;
	public GameManager manager;
	// I need our main GameManager to be passed along into MusicManager to access the audio
	// that is attached to it

    public int aggro = 1;
    public int capAggro = 20;
    public int baseAggro = 1;
    //public bool readyForDrop = false;
    public bool dropped = false;

    public float aggroLossTimer = 0f;
    public const float aggroLossTime = 15f;

    public int thresholdAggro = 10;
    public float thresholdTime;
    public float thresholdClock = 0f;
	public float BPM = 189f;
	public float measureLength;

    public float bgSpeed = 3f;

    private GameObject sky; // back-most layer of the background (probably only temporary)
    private GameObject street;

    private GameObject plotFolder;
    private GameObject sceneryFolder;
    private GameObject enemyFolder;
    private GameObject bundleFolder;
    public GameObject projectileFolder;

    private int consecutiveEmptyPlots = 0;
    
    public int totalMoney = 0;

    private GUIStyle style;
    private Canvas canvas;
    private Image[] hearts;
    private Sprite fullHeart;
    private Sprite emptyHeart;
    private Text score;
    private Text perPaperText, weatherText, temperatureText;
    private Text howManyText;
    private Image frontPaper;
    private Image paper2, paper3;
    private Image morePaper;

    private float bundleClock = 0f;

	public bool playerDead = false;
    public bool paused = false;

	// Use this for initialization
	void Start () {
        bundleFolder = new GameObject();
        enemyFolder = new GameObject();
        enemyFolder.name = "Enemy Folder";
        projectileFolder = new GameObject();
        projectileFolder.name = "Projectile Folder";
        this.transform.position = Vector3.zero;

        // GUI!!!
        canvas = Instantiate(Resources.Load<Canvas>("Prefabs/Canvas"));
        hearts = new Image[3];
        Image heartprefab = Resources.Load<Image>("Prefabs/Heart");
        fullHeart = Resources.Load<Sprite>("Sprites/heartFull");
        emptyHeart = Resources.Load<Sprite>("Sprites/heartEmpty");
        for (int i = 0; i < 3; i++)
        {
            hearts[i] = Instantiate(heartprefab);
            hearts[i].transform.SetParent(canvas.transform, false);
            hearts[i].rectTransform.anchoredPosition = new Vector2(40, -40);
            hearts[i].rectTransform.anchoredPosition += Vector2.right * 50 * i;
        }
        Image pigPrefab = Resources.Load<Image>("Prefabs/Pig");
        Image pig = Instantiate(pigPrefab);
        pig.rectTransform.SetParent(canvas.transform, false);
        score = pig.GetComponentInChildren<Text>();
        Image frontPaperPrefab = Resources.Load<Image>("Prefabs/Paper");
        frontPaper = Instantiate(frontPaperPrefab);
        frontPaper.rectTransform.SetParent(canvas.transform, false);
        perPaperText = frontPaper.transform.Find("issueprice")
            .gameObject.GetComponent<Text>();
        weatherText = frontPaper.transform.Find("weatherdescrip")
            .gameObject.GetComponent<Text>();
        temperatureText = frontPaper.transform.Find("Temperature")
            .gameObject.GetComponent<Text>();
        Image behindPaper = Resources.Load<Image>("Prefabs/BehindPaper");
        paper2 = Instantiate(behindPaper);
        paper2.rectTransform.SetParent(canvas.transform, false);
        paper2.rectTransform.SetAsFirstSibling();
        paper2.rectTransform.anchoredPosition +=
            (Vector2.right + Vector2.down) * 10;
        paper3 = Instantiate(behindPaper);
        paper3.rectTransform.SetParent(canvas.transform, false);
        paper3.rectTransform.SetAsFirstSibling();
        paper3.rectTransform.anchoredPosition +=
            (Vector2.right + Vector2.down) * 20;
        Image morePaperPrefab = Resources.Load<Image>("Prefabs/MorePaper");
        morePaper = Instantiate(morePaperPrefab);
        morePaper.rectTransform.SetParent(canvas.transform, false);
        morePaper.rectTransform.SetAsFirstSibling();
        howManyText = morePaper.transform.Find("HowMany").gameObject
            .GetComponent<Text>();

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
        street = makeBackground("street" + level, 2);

        sceneryFolder = new GameObject();
        sceneryFolder.name = "Scenery";
        plotFolder = new GameObject();
        plotFolder.name = "Plots";

		// Music
		if (level == 1) {
			BPM = 189;
		} else if (level == 2) {
			BPM = 122;
		} else if (level == 3) {
			BPM = 174;
		}
		measureLength = (4f * 60f) / BPM;
		thresholdTime = measureLength;// may be redundant

		// Level ending stuff
		numSide = 0;
		numPlots = 0;

        style = new GUIStyle();
        style.fontSize = 20;
        style.normal.textColor = Color.white;

        Time.timeScale = 1;
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
        level = whichLevel;
        if (level == 1) { levelName = "Rural"; }
        if (level == 2) { levelName = "Suburban"; }
        if (level == 3) { levelName = "Urban"; }
    }

	// Update is called once per frame
	void Update () {

        plotClock += Time.deltaTime;
        sceneryClock += Time.deltaTime;
        bundleClock += Time.deltaTime;
		if (numPlots < plotCap) {
			// All spawning behavior
			if (plotClock >= 6 / bgSpeed) {
				plotClock = 0 + Time.deltaTime;
				if (consecutiveEmptyPlots >= 2 || Random.value > 0.5) {
					spawnPlot (1);
					consecutiveEmptyPlots = 0;
					numPlots++;
				} else {
					spawnPlot (2);
					consecutiveEmptyPlots++;
				}
			}
			if (sceneryClock >= (6 / bgSpeed / 4)) {
				sceneryClock = 0 + Time.deltaTime;
				spawnLine ();
				spawnSidewalk ();
			}
			if (bundleClock >= 10) {
				Bundle bundle = (new GameObject ()).AddComponent<Bundle> ();
				bundle.init (this);
				bundleClock = 0f;
			}

			spawnCows();
			spawnFarmers();

		} else { 
			if (numSide < 5) {
				if (sceneryClock >= (6 / bgSpeed / 4)) {
					sceneryClock = 0 + Time.deltaTime;
					spawnLine ();
					spawnSidewalk ();
					numSide++;
				}
			}
		}
		if (player.hp > 0) {
			manageAggro ();
		}
        
        updateGUI();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!paused)
                pause();
            else
                unpause();
        }
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
        GameObject obj = new GameObject();
        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
        Scenery tile = obj.AddComponent<Scenery>();
        obj.transform.parent = sceneryFolder.transform;
        obj.name = "Sidewalk";
        tile.transform.localScale = new Vector3(1, 1, 0);
        tile.transform.position = new Vector3(rdWidth/2f+1, 1.5f, 1);
        tile.init("sidewalk" + levelName, bgSpeed, this);
    }

    private void spawnLine()
    {
        GameObject obj = new GameObject();
        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
        Scenery line = obj.AddComponent<Scenery>();
        obj.transform.parent = sceneryFolder.transform;
        obj.name = "Line";
        line.transform.localScale = new Vector3(1, 1, 0);
        line.transform.position = new Vector3(rdWidth/2f+1, -1, 1);
        line.init("streetLine", bgSpeed, this);
    }

    private GameObject makeBackground(string image, int layer)
    {
        GameObject obj = new GameObject();
        SpriteRenderer sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/" + image);
        obj.transform.localScale = new Vector3(1, 1, 1);
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
        thresholdClock += Time.deltaTime;
        
		// Checking to see if the measure is up, for dropping and undropping
		if (thresholdClock > thresholdTime)
        {
			if (aggro >= thresholdAggro && !dropped) 
				dropped = true;
			if (aggro < thresholdAggro && dropped)
				dropped = false;
			thresholdClock = 0;
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
        
        if (Cow.spawnClock > Cow.spawnNext)
        {
            Cow.spawnClock -= Cow.spawnNext;
            GameObject go = new GameObject();
            go.transform.parent = enemyFolder.transform;
            go.transform.position = spawnPt;
            Cow cow = go.AddComponent<Cow>();
            if (dropped)
            {
                cow.init(this, true);
            }
            else
            {
                cow.init(this, false);
            }
            Cow.spawnNext = (Cow.minTimeBase + Cow.minTimeAggro * aggro) +
                (Cow.spreadTimeBase + Cow.spreadTimeAggro * aggro) *
                Random.value;
        }
        Cow.spawnClock += Time.deltaTime;
    }

    private void spawnFarmers() {
        float place = Random.value * rdHeight - rdHeight / 2;
        Vector3 spawnPt = new Vector3(spawnPtPad + (rdWidth / 2), place, 0);

        if (Farmer.spawnClock > Farmer.spawnNext)
        {
            Farmer.spawnClock -= Farmer.spawnNext;
            GameObject go = new GameObject();
            go.transform.parent = enemyFolder.transform;
            go.transform.position = spawnPt;
            Farmer farmer = go.AddComponent<Farmer>();
            if (dropped)
                farmer.init(this, true);
            else
                farmer.init(this, false);
            Farmer.spawnNext = (Farmer.minTimeBase + Farmer.minTimeAggro * aggro) +
                (Farmer.spreadTimeBase + Farmer.spreadTimeAggro * aggro) *
                Random.value;
        }
        Farmer.spawnClock += Time.deltaTime;
    }

    void OnGUI()
    {
		if (!playerDead) {
            //GUI.Label(new Rect(Screen.width - 110, Screen.height - 100, 110, 50), "Aggro: " + aggro, style);
            //GUI.Label(new Rect(25, Screen.height - 50, 110, 50), "Papers: " + player.papers, style);
            //GUI.Label(new Rect(Screen.width - 110, Screen.height - 50, 110, 50), "$/paper: " + (aggro * 50), style);
        } else {
			if ((GUI.Button (new Rect ((Screen.width / 2) - 50, (Screen.height / 2) - 50, 200, 50), "Press R to restart"))
				|| Input.GetKeyDown(KeyCode.R)) {
				manager.resetLevel (manager.level);
                Time.timeScale = 1;

				// sound 
				manager.PlayEffect(manager.menu);
			}
            if ((GUI.Button(new Rect((Screen.width / 2) - 50, (Screen.height / 2) - 0, 200, 50), "Continue (Lose Money)"))
                || Input.GetKeyDown(KeyCode.R))
            {
                player.hp = player.maxhp;
                totalMoney = totalMoney / 2;
                aggro = aggro / 3;
                Destroy(enemyFolder);
                enemyFolder = new GameObject();
                Destroy(projectileFolder);
                projectileFolder = new GameObject();
                playerDead = false;
                Time.timeScale = 1;

                // sound 
                manager.PlayEffect(manager.menu);
            }
            if (level < 3) {
				if (GUI.Button (new Rect ((Screen.width / 2) - 50, (Screen.height / 2) + 50, 200, 50), "Skip to next level")) {
					manager.resetLevel (manager.level + 1);

					// sound
					manager.PlayEffect(manager.menu);
				}
			}
		}
        if(paused)
        {
            if ((GUI.Button(new Rect((Screen.width / 2) - 50, (Screen.height / 2) - 50, 200, 50), "Resume game (ESC)")))
            {
                unpause();
            }
            if (GUI.Button(new Rect((Screen.width / 2) - 50, (Screen.height / 2), 200, 50), "Skip to next level"))
            {
                unpause();
                manager.resetLevel(manager.level + 1);
                // sound
                manager.PlayEffect(manager.menu);
            }
            if (GUI.Button(new Rect((Screen.width / 2) - 50, (Screen.height / 2) + 50, 200, 50), "Exit to menu"))
            {
                manager.loadMainMenu();
                // sound
                manager.PlayEffect(manager.menu);
            }
        }
    }

    private void updateGUI()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            Image im = hearts[i].GetComponent<Image>();
            if (i + 1 > player.hp)
                im.sprite = emptyHeart;
            else
                im.sprite = fullHeart;
            if (!dropped)
            {
                im.color = new Color(1, 1, 1,
                    1 - (Mathf.Sin(2 * Mathf.PI *
                    (Time.timeSinceLevelLoad % 2) / 2))/2);
            }
            else
                im.color = Color.white;
        }
        score.text = "$" + string.Format("{0:#.00}", totalMoney /100f);
        perPaperText.text = "$" + string.Format(
            "{0:#.00}", aggro * 50 / 100f);
        temperatureText.text = (67 + aggro * 3) + "\u00B0F";
        if (dropped) weatherText.text = "Chaotic and\nStormy";
        else weatherText.text = "Clear and\nCheerful";
        howManyText.text = "" + player.papers;
        frontPaper.gameObject.SetActive(player.papers >= 1);
        paper2.gameObject.SetActive(player.papers >= 2);
        paper3.gameObject.SetActive(player.papers >= 3);
        morePaper.gameObject.SetActive(player.papers >= 4);

    }

    public void drop()
    {
        dropped = true;
    }

	public void undrop() {
		dropped = false;
	}

	public void reduceAggro(int reduce) {
		aggro -= reduce;
	}

    public void annihilate()
    {
        Destroy(sky);
        Destroy(street);
        Destroy(enemyFolder);
        Destroy(projectileFolder);
        Destroy(plotFolder);
        Destroy(sceneryFolder);
        Destroy(bundleFolder);
        Destroy(canvas.GetComponentInChildren<CanvasScaler>());
        Destroy(canvas.GetComponentInChildren<GraphicRaycaster>());
        Destroy(canvas);
        if (player != null) Destroy(player.gameObject);
        Destroy(this.gameObject);
    }

    public void pause()
    {
        paused = true;
        Time.timeScale = 0;
        manager.PlayEffect(manager.menu);
    }

    public void unpause()
    {
        paused = false;
        Time.timeScale = 1;
        manager.PlayEffect(manager.menu);
    }

    private GameObject cow = Resources.Load<GameObject>("Prefabs/Cow");
    private GameObject farmer = Resources.Load<GameObject>("Prefabs/Farmer");
    private GameObject madCow = Resources.Load<GameObject>("Prefabs/MadCow");
    private GameObject angryFarmer = Resources.Load<GameObject>("Prefabs/AngryFarmer");
}
