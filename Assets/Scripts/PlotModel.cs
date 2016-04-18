using UnityEngine;
using System.Collections;

public class PlotModel : MonoBehaviour {

    private int type;
    private Plot owner;
    private Material mat;
    private int boxNum; //number for mailbox texture to make changing it easier

    public void init(int type, Plot owner)
    {
        this.type = type;
        this.owner = owner;

        transform.parent = owner.transform;

        System.Random rnd = new System.Random();

        mat = GetComponent<Renderer>().material;
        mat.shader = Shader.Find("Sprites/Default");
        mat.color = new Color(1, 1, 1);

        if (type == 0)
        {
            mat.mainTexture = Resources.Load<Texture2D>("Sprites/house" + rnd.Next(1,3));
            transform.localPosition = new Vector3(0, 0, 0);
            //gameObject.tag = "house";
            gameObject.name = "house";
        }
        else if (type == 1)
        {
            boxNum = rnd.Next(1, 4);
            mat.mainTexture = Resources.Load<Texture2D>("Sprites/mailbox"+ boxNum + "open");
            transform.localPosition = new Vector3(0, 0, 0);
            //gameObject.tag = "mailbox";
            gameObject.name = "mailbox";
        }
        else if (type == 2)
        {
            mat.mainTexture = Resources.Load<Texture2D>("Sprites/fenceWood1");
            transform.localPosition = new Vector3(0, 0, 0);
            //gameObject.tag = "scenery";
            gameObject.name = "scenery";
        }
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (type == 1)
        {
            if (!other.gameObject.CompareTag("paper")) return;
            Destroy(other.gameObject);
            owner.manager.totalMoney += owner.manager.aggro * 50;
			owner.manager.aggro--;
            mat.mainTexture = Resources.Load<Texture2D>("Sprites/mailbox" + boxNum + "closed");
        }
    }
    
}
