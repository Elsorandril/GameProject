using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
	private int Score;
	private int Level;
	private int offX;
	private int offY;
	
    // Start is called before the first frame update
    void Start()
    {
        Score=0;
		Level=1;
		offX=3;
		offY=3;
		GameObject ry=GameObject.Find( "Rydia" );
		ry.transform.localPosition =getCoord(0,0);
    }

    // Update is called once per frame
    void Update()
    {
		Grid[] grid=GameObject.FindObjectsOfType( typeof(Grid) ) as Grid[];
		if (Input.touchCount > 0)
        {
			Touch touch = Input.GetTouch(0);
			Vector2 pos = touch.position;
			//pos.x = (pos.x - width) / width;
			//pos.y = (pos.y - height) / height;
			//position = new Vector3(-pos.x, pos.y, 0.0f);
			Debug.Log("("+pos.x+","+pos.y+")");
		}
    }
	
	public Vector3 getCoord(int x, int y){
		Grid[] grid=GameObject.FindObjectsOfType( typeof(Grid) ) as Grid[];
		return grid[0].GetCellCenterLocal(new  Vector3Int(x-offX,y-offY,0));
	}
	
	
}
