using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
	private int Score;
	private int Level;
	private int offX;
	private int offY;
	private int width;
	private int height;
	private Vector3 position;
	private GameObject[][] map;
	private Vector3 selected;
	public int mapSizeX;
	public int mapSizeY;
	
    // Start is called before the first frame update
    void Start()
    {
        Score=0;
		Level=1;
		offX=3;
		offY=3;
		this.map=new GameObject[mapSizeX][];
		for(int i=0;i<mapSizeX-1;i++){
			this.map[i]=new GameObject[mapSizeY];
		}
		GameObject ry=GameObject.Find( "Rydia" );
		this.map[0][0]=ry;
		ry.transform.localPosition =getCoord(0,0);
    }

    // Update is called once per frame
    void Update()
    {
		Grid[] grid=GameObject.FindObjectsOfType( typeof(Grid) ) as Grid[];
		//if (Input.touchCount > 0)
		if (Input.GetButtonDown("Fire1"))
        {
			//Touch touch = Input.GetTouch(0);
			//Vector2 pos = touch.position;
			Vector2 pos = Input.mousePosition;
			Debug.Log("("+pos.x+","+pos.y+")");
			position = new Vector3(-pos.x, pos.y, 0.0f);
			Debug.Log("("+pos.x+","+pos.y+")");
			if(!positionToCell(pos).Equals(Vector3.positiveInfinity)){
				Debug.Log("Clicked");
				Vector3 temp=positionToCell(pos);
				Debug.Log("Temp : ("+temp.x+","+temp.y+")");
				if(selected==Vector3.positiveInfinity){
					this.selected=temp;
				}
				else{
					this.map[(int)temp.x][(int)temp.y]=this.map[(int)this.selected.x][(int)this.selected.y];
					this.map[(int)this.selected.x][(int)this.selected.y]=null;
					this.selected=Vector3.positiveInfinity;
				}
			}
		}
		
    }
	
	public Vector3 getCoord(int x, int y){
		Grid[] grid=GameObject.FindObjectsOfType( typeof(Grid) ) as Grid[];
		return grid[0].GetCellCenterLocal(new  Vector3Int(x-offX,y-offY,0));
	}
	
	public Vector3 positionToCell(Vector2 pos){
		Grid[] grid=GameObject.FindObjectsOfType( typeof(Grid) ) as Grid[];
		for(int i=0;i<6;i++){
			for(int j=0;j<6;j++){
				grid[0].GetBoundsLocal(new Vector3Int(i-offX,j-offY,0));
				Debug.Log("Temp : ("+grid[0].GetBoundsLocal(new Vector3Int(i-offX,j-offY,0)).max.x+","+grid[0].GetBoundsLocal(new Vector3Int(i-offX,j-offY,0)).max.y+")");
				if(grid[0].GetBoundsLocal(new Vector3Int(i-offX,j-offY,0)).Contains(pos)){
					Debug.Log("Contained");
					return new Vector3(i-offX,j-offY,0);
				}
			}
		}
		return Vector3.positiveInfinity;
	}
	
	
}
