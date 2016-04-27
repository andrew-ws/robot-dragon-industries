using UnityEngine;
using System.Collections;

public class PlotModel : MonoBehaviour {

    private int type;
    private Plot owner;
    protected SpriteRenderer sr;
    private int boxNum; //number for mailbox texture to make changing it easier
    private string levelName;

    public void init(int type, Plot owner)
    {
        this.type = type;
        this.owner = owner;

        transform.parent = owner.transform;
        levelName = owner.manager.levelName;
        sr = GetComponent<SpriteRenderer>();

        if (type == 0)
        {
            sr.sprite = Resources.Load<Sprite>("Sprites/house" + levelName + Random.Range(1,4));
            transform.localPosition = new Vector3(0, 0, 0);
            gameObject.name = "house";
        }
        else if (type == 1)
        {
            sr.sprite = Resources.Load<Sprite>("Sprites/fence"+ levelName);
            transform.localPosition = new Vector3(0, 0, 0);
            gameObject.name = "fence";
        }
        else if (type == 2)
        {
            sr.sprite = Resources.Load<Sprite>("Sprites/bush" + owner.manager.level);
            transform.localPosition = new Vector3(0, 0, 0);
            gameObject.name = "bush";
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
}
