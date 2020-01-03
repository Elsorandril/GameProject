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
	private GameObject[,] map;
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
		this.map=new GameObject[mapSizeX,mapSizeY];
		/*for(int i=0;i<mapSizeX-1;i++){
			this.map[i]=new GameObject[mapSizeY];
		}*/
		for(int i=0;i<this.mapSizeX;i++){
			for(int j=0;j<this.mapSizeY;j++){
				this.map[i,j]=null;
			}
		}
		this.selected=Vector3.positiveInfinity;
		GameObject ry=GameObject.Find( "Rydia" );
		this.map[0,0]=ry;
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
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Debug.Log("("+pos.x+","+pos.y+")");
			Vector3 temp=positionToCell(pos);
			if(temp.x+offX>=0&&temp.x+offX<=mapSizeX&&temp.y+offY>=0&&temp.y+offY<=mapSizeY){
				Debug.Log("Clicked");
				Debug.Log("Temp : ("+(temp.x+offX)+","+(temp.y+offY)+")");
				if(selected.Equals(Vector3.positiveInfinity)){
					Debug.Log("Temp Selected Only : "+this.selected);
					this.selected=temp;
				}
				else{
					
					Debug.Log("Temp Selected : "+this.selected);
					Debug.Log("Temp temp: "+temp);
					this.map[(int)temp.x+offX,(int)temp.y+offY]=this.map[(int)this.selected.x+offX,(int)this.selected.y+offY];
					this.map[(int)this.selected.x+offX,(int)this.selected.y+offY]=null;
					this.selected=Vector3.positiveInfinity;
				}
			}
		}
		updateMap();
		
    }
	public void updateMap(){
		for(int i=0;i<this.mapSizeX;i++){
			for(int j=0;j<this.mapSizeY;j++){
				if(this.map[i,j]!=null){
					this.map[i,j].transform.localPosition =getCoord(i,j);
				}
			}
		}
	}
	
	public Vector3 getCoord(int x, int y){
		Grid[] grid=GameObject.FindObjectsOfType( typeof(Grid) ) as Grid[];
		return grid[0].GetCellCenterWorld(new  Vector3Int(x-offX,y-offY,0));
	}
	
	public Vector3 positionToCell(Vector3 pos){
		Grid[] grid=GameObject.FindObjectsOfType( typeof(Grid) ) as Grid[];
		Vector3Int temp=grid[0].WorldToCell(pos);
		Debug.Log("Temp2 : "+temp);
		return new Vector3(temp.x,temp.y,temp.z);
	}
	
	
}
