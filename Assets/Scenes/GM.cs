using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
	private int Score;
	private int Level;
	private int width;
	private int height;
	private GameObject[,] map;
	private GameObject[,] moveMap;
	private Vector3 selected;
	private LinkedList<Vector3Int> selectedMoves;
	public int mapSizeX;
	public int mapSizeY;
	public Vector3 offset;
	
	
    void Start()
    {
        Score=0;
		Level=1;
		this.offset=new Vector3(3,3,0);
		this.map=new GameObject[mapSizeX,mapSizeY];
		this.moveMap=new GameObject[mapSizeX,mapSizeY];
		for(int i=0;i<this.mapSizeX;i++){
			for(int j=0;j<this.mapSizeY;j++){
				this.map[i,j]=null;
				this.moveMap[i,j]=Instantiate(Resources.Load<GameObject>("Prefabs/blueTintPrefab"), getCoordCenter(i, j)+(new Vector3(0,0,1)), Quaternion.identity);
			}
		}
		this.selected=Vector3.positiveInfinity;
		GameObject ry=GameObject.Find( "Rydia" );
		this.map[0,0]=ry;
		GameObject ry2=GameObject.Find( "R2D2" );
		this.map[0,1]=ry2;
    }

	
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
			Vector3 temp=positionToCell(pos)+this.offset;
			if(inBound(temp)){
				Debug.Log("Clicked");
				Debug.Log("Temp : ("+(temp.x)+","+(temp.y)+")");
				if(selected.Equals(Vector3.positiveInfinity)){
					if(!empty(temp)){
						Debug.Log("Temp Selected Only : "+this.selected);
						this.selected=temp;
						this.selectedMoves=possibleMovesInRange(temp, 3);
					}
				}
				else{
					if(!this.selected.Equals(temp)&&this.selectedMoves.Contains(Vector3Int.FloorToInt(temp))){
						Debug.Log("Temp Selected : "+this.selected);
						Debug.Log("Temp temp: "+temp);
						if(this.map[(int)temp.x,(int)temp.y].tag=="Ennemies"){
							this.map[(int)temp.x,(int)temp.y]=this.map[(int)this.selected.x,(int)this.selected.y];
							this.map[(int)this.selected.x,(int)this.selected.y]=null;
							this.selected=Vector3.positiveInfinity;
							this.selectedMoves.Clear();
						}
						else if(this.map[(int)temp.x,(int)temp.y].tag=="Allies"){
							this.selected=temp;
							this.selectedMoves=possibleMovesInRange(temp, 3);
						}
						else{
							
						}
					}
					else{
						this.selected=Vector3.positiveInfinity;
						this.selectedMoves.Clear();
					}
				}
			}
		}
		updateMap();
		
    }
	
	public void updateMap(){
		for(int i=0;i<this.mapSizeX;i++){
			for(int j=0;j<this.mapSizeY;j++){
				this.moveMap[i,j].transform.localPosition=(getCoordCenter(i,j)+new Vector3(0,0,1));
				if(this.map[i,j]!=null){
					this.map[i,j].transform.localPosition =getCoordCenter(i,j);
				}
			}
		}
		if(this.selectedMoves!=null){
			foreach(Vector3Int Vi in this.selectedMoves){
				this.moveMap[Vi.x,Vi.y].transform.localPosition=(getCoordCenter(Vi.x,Vi.y)+new Vector3(0,0,-1));
			}
		}
	}
	
	public bool empty(Vector3 pos){
		return (this.map[(int)pos.x,(int)pos.y]==null);
	}
	
	public Vector3 getCoordCenter(int x, int y){
		Grid[] grid=GameObject.FindObjectsOfType( typeof(Grid) ) as Grid[];
		return grid[0].GetCellCenterWorld(new  Vector3Int(x,y,0)-Vector3Int.FloorToInt(this.offset));
	}
	
	public Vector3 positionToCell(Vector3 pos){
		Grid[] grid=GameObject.FindObjectsOfType( typeof(Grid) ) as Grid[];
		Vector3Int temp=grid[0].WorldToCell(pos);
		return new Vector3(temp.x,temp.y,temp.z);
	}
	
	public LinkedList<Vector3Int> possibleMovesInRange(Vector3 actual, int range){
		LinkedList<Vector3Int> ll=new LinkedList<Vector3Int>();
		for(int i=(0-range);i<range;i++){
			for(int j=0;j<(range-Mathf.Abs(i));j++){
				Debug.Log("i: "+i+" j: "+j);
				if(inBound(actual+(new Vector3(i,-j,0)))){
					ll.AddFirst(Vector3Int.FloorToInt(actual+(new Vector3(i,-j,0))));
				}
				if(inBound(actual+(new Vector3(i,j,0)))){
					ll.AddFirst(Vector3Int.FloorToInt(actual+(new Vector3(i,j,0))));
				}
			}
		}
		return ll;
	}
	
	public bool inBound(Vector3 pos){
		return (pos.x>=0&&pos.x<(this.mapSizeX))&&(pos.y>=0&&pos.y<(this.mapSizeY));
	}
	
	public bool gameOver(){
		return GameObject.FindGameObjectsWithTag("Allies").Length ==0;
	}
	
	public bool winLevel(){
		return GameObject.FindGameObjectsWithTag("Ennemies").Length ==0;
	}
}
