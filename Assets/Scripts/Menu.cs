using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

    SpriteRenderer sr;
    FontStyle style;
    GameManager manager;

	// Use this for initialization
	public void init(GameManager manager) {
        this.manager = manager;

        //since this comes before the level, we have to recalculate to position the menu image properly
        Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        cam.orthographicSize = (LevelManager.rdWidth / cam.aspect) / 2;
        float camy = cam.orthographicSize - LevelManager.rdHeight / 2 - LevelManager.rdMargin;
        float backgroundHt = LevelManager.rdWidth * (7f / 8f);
        float backgroundy = camy - (backgroundHt - (cam.orthographicSize * 2)) / 2;

        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/title");
        gameObject.transform.localPosition = new Vector3(0, backgroundy, 0);//GetComponent<Camera>().orthographicSize
        gameObject.transform.localScale = new Vector3(1, 1, 1);
        gameObject.name = "Menu";
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width/3-100, 75, 200, 50), "Start"))
        {
            //begin game
        }
        if (GUI.Button(new Rect(Screen.width/3*2-100, 75, 200, 50), "Quit"))
        {
            //begin game
        }
    }
}
