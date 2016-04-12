using UnityEngine;
using System.Collections;

public class Cow : MonoBehaviour {

    public Vector3 velocity = new Vector3(-1, 0, 0);
    public LevelManager lm;

    public SpriteRenderer sr;

	// Use this for initialization
	void Start () {
        this.transform.localScale = new Vector3(0.18f, 0.18f, 1f);
        lm = GameObject.Find("GameObject").GetComponent<GameManager>().lm;
        sr = gameObject.AddComponent<SpriteRenderer>();
        sr.sprite = Resources.Load<Sprite>("Sprites/cow");
    }
	
	// Update is called once per frame
	void Update () {
        this.transform.position += (velocity + lm.bgSpeed * Vector3.left)
            * Time.deltaTime;
	}
}
