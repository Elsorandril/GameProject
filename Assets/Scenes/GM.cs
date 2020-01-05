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
		ry.transform.localPosition =getCoordCenter(0,0);
    }

	
    void Update()
    {
		Grid[] grid=GameObject.FindObjectsOfType( typeof(Grid) ) as Grid[];
		if (Input.touchCount > 0)
		//if (Input.GetButtonDown("Fire1"))
        {
			Touch touch = Input.GetTouch(0);
			Vector2 pos = touch.position;
			//Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
						this.map[(int)temp.x,(int)temp.y]=this.map[(int)this.selected.x,(int)this.selected.y];
						this.map[(int)this.selected.x,(int)this.selected.y]=null;
						this.selected=Vector3.positiveInfinity;
						this.selectedMoves.Clear();
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
		Debug.Log("Temp2 : "+temp);
		return new Vector3(temp.x,temp.y,temp.z);
	}
	
	public LinkedList<Vector3Int> possibleMovesInRange(Vector3 actual, int range){
		LinkedList<Vector3Int> ll=new LinkedList<Vector3Int>();
		for(int i=(0-range);i<range;i++){
			for(int j=0;j<(range-Mathf.Abs(i));j++){
				Debug.Log("i: "+i+" j: "+j);
				if(inBound(actual+(new Vector3(i,-j,0)))){
					Debug.Log("Added i: "+i+" j: "+(-j));
					ll.AddFirst(Vector3Int.FloorToInt(actual+(new Vector3(i,-j,0))));
				}
				if(inBound(actual+(new Vector3(i,j,0)))){
					Debug.Log("Added i: "+i+" j: "+(j));
					ll.AddFirst(Vector3Int.FloorToInt(actual+(new Vector3(i,j,0))));
				}
			}
		}
		return ll;
	}
	
	public bool inBound(Vector3 pos){
		//Debug.Log("Temp3 : "+pos);
		return (pos.x>=0&&pos.x<(this.mapSizeX))&&(pos.y>=0&&pos.y<(this.mapSizeY));
	}
	
	/*public LinkedList<Coord> getDeplacemnts(Pawn[][] p){
        LinkedList<Coord> lc=new LinkedList<Coord>();
        if(Type.SOLDIERR.isThispiece(this.type)){
            if(this.type.isRed()){
                if(this.inBlackCamp(this.c)){
                    if(this.c.add(-1,0).isBetween(-1,-1,10,9)&&!getPawnFromBoard(p,this.c.add(-1,0)).type.isRed()){
                        lc.add(this.c.add(-1,0));
                    }
                    if(this.c.add(0,-1).isBetween(-1,-1,10,9)&&!getPawnFromBoard(p,this.c.add(0,-1)).type.isRed()){
                        lc.add(this.c.add(0,-1));
                    }
                    if(this.c.add(0,1).isBetween(-1,-1,10,9)&&!getPawnFromBoard(p,this.c.add(0,1)).type.isRed()){
                        lc.add(this.c.add(0,1));
                    }
                }else{
                    if(this.c.add(-1,0).isBetween(-1,-1,10,9)&&!getPawnFromBoard(p,this.c.add(-1,0)).type.isRed()){
                        lc.add(this.c.add(-1,0));
                    }
                }
            }else{
                if(this.inRedCamp(this.c)){
                    if(this.c.add(1,0).isBetween(-1,-1,10,9)&&!getPawnFromBoard(p,this.c.add(1,0)).type.isBlack()){
                        lc.add(this.c.add(1,0));
                    }
                    if(this.c.add(0,-1).isBetween(-1,-1,10,9)&&!getPawnFromBoard(p,this.c.add(0,-1)).type.isBlack()){
                        lc.add(this.c.add(0,-1));
                    }
                    if(this.c.add(0,1).isBetween(-1,-1,10,9)&&!getPawnFromBoard(p,this.c.add(0,1)).type.isBlack()){
                        lc.add(this.c.add(0,1));
                    }
                }else{
                    if(this.c.add(1,0).isBetween(-1,-1,10,9)&&!getPawnFromBoard(p,this.c.add(1,0)).type.isBlack()){
                        lc.add(this.c.add(1,0));
                    }
                }
            }
        }
        else if(Type.CANONR.isThispiece(this.type)){
            LinkedList<LinkedList<Coord>> possibleMoves=new LinkedList<LinkedList<Coord>>();
            possibleMoves.add(this.getAllMoveLine(-1,0));
            possibleMoves.add(this.getAllMoveLine(1,0));
            possibleMoves.add(this.getAllMoveLine(0,-1));
            possibleMoves.add(this.getAllMoveLine(0,1));
            for (LinkedList<Coord> ct:possibleMoves){
                boolean Jumping=false;
                if(ct!=null){
                    for (Coord ctt:ct){
                        if(!Jumping&&this.getPawnFromBoard(p,this.c.add(ctt)).isEmpty()){
                            lc.add(this.c.add(ctt));
                        }
                        else if(Jumping&&!this.getPawnFromBoard(p,this.c.add(ctt)).isEmpty()&&!this.getPawnFromBoard(p,this.c.add(ctt)).type.isColor(this.type)){
                            lc.add(this.c.add(ctt));
                            break;
                        }
                        else{Jumping=true;}
                    }
                }
            }
        }
        else if(Type.CHARIOTR.isThispiece(this.type)){
            LinkedList<LinkedList<Coord>> possibleMoves=new LinkedList<LinkedList<Coord>>();
            possibleMoves.add(this.getAllMoveLine(-1,0));
            possibleMoves.add(this.getAllMoveLine(1,0));
            possibleMoves.add(this.getAllMoveLine(0,-1));
            possibleMoves.add(this.getAllMoveLine(0,1));
            for (LinkedList<Coord> ct:possibleMoves){
                if(ct!=null){
                    for (Coord ctt:ct){
                        if(this.getPawnFromBoard(p,this.c.add(ctt)).isEmpty()){
                            lc.add(this.c.add(ctt));
                        }
                        else if(!this.getPawnFromBoard(p,this.c.add(ctt)).type.isColor(this.type)){
                            lc.add(this.c.add(ctt));
                            break;
                        }
                        else{break;}
                    }
                }
            }
        }
        else if(Type.HORSER.isThispiece(this.type)){
            Coord possibleMoves[][]={{new Coord(-2,-1),new Coord(-2,1)},
                    {new Coord(-1,-2),new Coord(1,-2)},
                    {new Coord(-1,2),new Coord(1,2)},
                    {new Coord(2,-1),new Coord(2,1)}};
            Coord toCheck[]={new Coord(-1,0),new Coord(0,-1),new Coord(0,1),new Coord(1,0)};
            for(int i=0;i<toCheck.length;i++){
                if(this.c.add(toCheck[i]).isBetween(-1,-1,10,9)&&getPawnFromBoard(p,this.c.add(toCheck[i])).isEmpty()){
                    for(int j=0;j<possibleMoves[i].length;j++){
                        if(this.c.add(possibleMoves[i][j]).isBetween(-1,-1,10,9)&&!getPawnFromBoard(p,this.c.add(possibleMoves[i][j])).type.isColor(this.type)){
                            lc.add(this.c.add(possibleMoves[i][j]));
                        }
                    }
                }
            }
        }
        else if(Type.ELEPHANTR.isThispiece(this.type)){
            if(this.type.isRed()){
                if(this.c.add(2,2).isBetween(4,-1,10,9)&&!getPawnFromBoard(p,this.c.add(2,2)).type.isRed()&&getPawnFromBoard(p,this.c.add(1,1)).isEmpty()){
                    lc.add(this.c.add(2,2));
                }
                if(this.c.add(-2,-2).isBetween(4,-1,10,9)&&!getPawnFromBoard(p,this.c.add(-2,-2)).type.isRed()&&getPawnFromBoard(p,this.c.add(-1,-1)).isEmpty()){
                    lc.add(this.c.add(-2,-2));
                }
                if(this.c.add(-2,2).isBetween(4,-1,10,9)&&!getPawnFromBoard(p,this.c.add(-2,2)).type.isRed()&&getPawnFromBoard(p,this.c.add(-1,1)).isEmpty()){
                    lc.add(this.c.add(-2,2));
                }
                if(this.c.add(2,-2).isBetween(4,-1,10,9)&&!getPawnFromBoard(p,this.c.add(2,-2)).type.isRed()&&getPawnFromBoard(p,this.c.add(1,-1)).isEmpty()){
                    lc.add(this.c.add(2,-2));
                }
            }else {
                if(this.c.add(2,2).isBetween(-1, -1, 5, 9)&&!getPawnFromBoard(p,this.c.add(2,2)).type.isBlack()&&getPawnFromBoard(p,this.c.add(1,1)).isEmpty()){
                    lc.add(this.c.add(2,2));
                }
                if(this.c.add(-2,-2).isBetween(-1, -1, 5, 9)&&!getPawnFromBoard(p,this.c.add(-2,-2)).type.isBlack()&&getPawnFromBoard(p,this.c.add(-1,-1)).isEmpty()){
                    lc.add(this.c.add(-2,-2));
                }
                if(this.c.add(-2,2).isBetween(-1, -1, 5, 9)&&!getPawnFromBoard(p,this.c.add(-2,2)).type.isBlack()&&getPawnFromBoard(p,this.c.add(-1,1)).isEmpty()){
                    lc.add(this.c.add(-2,2));
                }
                if(this.c.add(2,-2).isBetween(-1, -1, 5, 9)&&!getPawnFromBoard(p,this.c.add(2,-2)).type.isBlack()&&getPawnFromBoard(p,this.c.add(1,-1)).isEmpty()){
                    lc.add(this.c.add(2,-2));
                }
            }
        }
        else if(Type.ADVISORR.isThispiece(this.type)){
            if(this.type.isRed()){
                if(this.c.add(1,1).isBetween(6,2,10,6)&&!getPawnFromBoard(p,this.c.add(1,1)).type.isRed()){
                    lc.add(this.c.add(1,1));
                }
                if(this.c.add(-1,-1).isBetween(6,2,10,6)&&!getPawnFromBoard(p,this.c.add(-1,-1)).type.isRed()){
                    lc.add(this.c.add(-1,-1));
                }
                if(this.c.add(-1,1).isBetween(6,2,10,6)&&!getPawnFromBoard(p,this.c.add(-1,1)).type.isRed()){
                    lc.add(this.c.add(-1,1));
                }
                if(this.c.add(1,-1).isBetween(6,2,10,6)&&!getPawnFromBoard(p,this.c.add(1,-1)).type.isRed()){
                    lc.add(this.c.add(1,-1));
                }
            }else {
                if (this.c.add(1, 1).isBetween(-1, 2, 3, 6) && !getPawnFromBoard(p, this.c.add(1, 1)).type.isBlack()) {
                    lc.add(this.c.add(1, 1));
                }
                if (this.c.add(1, -1).isBetween(-1, 2, 3, 6) && !getPawnFromBoard(p, this.c.add(1, -1)).type.isBlack()) {
                    lc.add(this.c.add(1, -1));
                }
                if (this.c.add(-1, 1).isBetween(-1, 2, 3, 6) && !getPawnFromBoard(p, this.c.add(-1, 1)).type.isBlack()) {
                    lc.add(this.c.add(-1, 1));
                }
                if (this.c.add(-1, -1).isBetween(-1, 2, 3, 6) && !getPawnFromBoard(p, this.c.add(-1, -1)).type.isBlack()) {
                    lc.add(this.c.add(-1, -1));
                }
            }

        }
        else if(Type.GENERALR.isThispiece(this.type)){
            if(this.type.isRed()){
                if(this.c.add(1,0).isBetween(6,2,10,6)&&!getPawnFromBoard(p,this.c.add(1,0)).type.isRed()){
                    lc.add(this.c.add(1,0));
                }
                if(this.c.add(0,-1).isBetween(6,2,10,6)&&!getPawnFromBoard(p,this.c.add(0,-1)).type.isRed()){
                    lc.add(this.c.add(0,-1));
                }
                if(this.c.add(0,1).isBetween(6,2,10,6)&&!getPawnFromBoard(p,this.c.add(0,1)).type.isRed()){
                    lc.add(this.c.add(0,1));
                }
                if(this.c.add(-1,0).isBetween(6,2,10,6)&&!getPawnFromBoard(p,this.c.add(-1,0)).type.isRed()){
                    lc.add(this.c.add(-1,0));
                }
            }else{
                if(this.c.add(1,0).isBetween(-1,2,3,6)&&!getPawnFromBoard(p,this.c.add(1,0)).type.isBlack()){
                    lc.add(this.c.add(1,0));
                }
                if(this.c.add(0,-1).isBetween(-1,2,3,6)&&!getPawnFromBoard(p,this.c.add(0,-1)).type.isBlack()){
                    lc.add(this.c.add(0,-1));
                }
                if(this.c.add(0,1).isBetween(-1,2,3,6)&&!getPawnFromBoard(p,this.c.add(0,1)).type.isBlack()){
                    lc.add(this.c.add(0,1));
                }
                if(this.c.add(-1,0).isBetween(-1,2,3,6)&&!getPawnFromBoard(p,this.c.add(-1,0)).type.isBlack()){
                    lc.add(this.c.add(-1,0));
                }

            }
        }
		
        LinkedList<Coord> lc2=new LinkedList<Coord>();

        for (Coord co:lc){
			
            BoardManager bm=new BoardManager(p);
            bm.forcedMove(this.c,co);
            if(this.getType().isRed()&&bm.isRedInCheck()){
                Log.e("msgPerso","red in check");
            }
            else if(this.getType().isBlack()&&bm.isBlackInCheck()){
                Log.e("msgPerso","black in check");
            }else{
                lc2.add(co);
            }
        }
        return lc2;
    }
	
	
    public LinkedList<Coord> getDeplacemntsWithoutCheckTest(Pawn[][] p){
        LinkedList<Coord> lc=new LinkedList<Coord>();
        if(Type.SOLDIERR.isThispiece(this.type)){
            if(this.type.isRed()){
                if(this.inBlackCamp(this.c)){
                    if(this.c.add(-1,0).isBetween(-1,-1,10,9)&&!getPawnFromBoard(p,this.c.add(-1,0)).type.isRed()){
                        lc.add(this.c.add(-1,0));
                    }
                    if(this.c.add(0,-1).isBetween(-1,-1,10,9)&&!getPawnFromBoard(p,this.c.add(0,-1)).type.isRed()){
                        lc.add(this.c.add(0,-1));
                    }
                    if(this.c.add(0,1).isBetween(-1,-1,10,9)&&!getPawnFromBoard(p,this.c.add(0,1)).type.isRed()){
                        lc.add(this.c.add(0,1));
                    }
                }else{
                    if(this.c.add(-1,0).isBetween(-1,-1,10,9)&&!getPawnFromBoard(p,this.c.add(-1,0)).type.isRed()){
                        lc.add(this.c.add(-1,0));
                    }
                }
            }else{
                if(this.inRedCamp(this.c)){
                    if(this.c.add(1,0).isBetween(-1,-1,10,9)&&!getPawnFromBoard(p,this.c.add(1,0)).type.isBlack()){
                        lc.add(this.c.add(1,0));
                    }
                    if(this.c.add(0,-1).isBetween(-1,-1,10,9)&&!getPawnFromBoard(p,this.c.add(0,-1)).type.isBlack()){
                        lc.add(this.c.add(0,-1));
                    }
                    if(this.c.add(0,1).isBetween(-1,-1,10,9)&&!getPawnFromBoard(p,this.c.add(0,1)).type.isBlack()){
                        lc.add(this.c.add(0,1));
                    }
                }else{
                    if(this.c.add(1,0).isBetween(-1,-1,10,9)&&!getPawnFromBoard(p,this.c.add(1,0)).type.isBlack()){
                        lc.add(this.c.add(1,0));
                    }
                }
            }
        }
        else if(Type.CANONR.isThispiece(this.type)){
            LinkedList<LinkedList<Coord>> possibleMoves=new LinkedList<LinkedList<Coord>>();
            possibleMoves.add(this.getAllMoveLine(-1,0));
            possibleMoves.add(this.getAllMoveLine(1,0));
            possibleMoves.add(this.getAllMoveLine(0,-1));
            possibleMoves.add(this.getAllMoveLine(0,1));
            for (LinkedList<Coord> ct:possibleMoves){
                boolean Jumping=false;
                if(ct!=null){
                    for (Coord ctt:ct){
                        if(!Jumping&&this.getPawnFromBoard(p,this.c.add(ctt)).isEmpty()){
                            lc.add(this.c.add(ctt));
                        }
                        else if(Jumping&&!this.getPawnFromBoard(p,this.c.add(ctt)).isEmpty()&&!this.getPawnFromBoard(p,this.c.add(ctt)).type.isColor(this.type)){
                            lc.add(this.c.add(ctt));
                            break;
                        }
                        else{Jumping=true;}
                    }
                }
            }
        }
        else if(Type.CHARIOTR.isThispiece(this.type)){
            LinkedList<LinkedList<Coord>> possibleMoves=new LinkedList<LinkedList<Coord>>();
            possibleMoves.add(this.getAllMoveLine(-1,0));
            possibleMoves.add(this.getAllMoveLine(1,0));
            possibleMoves.add(this.getAllMoveLine(0,-1));
            possibleMoves.add(this.getAllMoveLine(0,1));
            for (LinkedList<Coord> ct:possibleMoves){
                if(ct!=null){
                    for (Coord ctt:ct){
                        if(this.getPawnFromBoard(p,this.c.add(ctt)).isEmpty()){
                            lc.add(this.c.add(ctt));
                        }
                        else if(!this.getPawnFromBoard(p,this.c.add(ctt)).type.isColor(this.type)){
                            lc.add(this.c.add(ctt));
                            break;
                        }
                        else{break;}
                    }
                }
            }
        }
        else if(Type.HORSER.isThispiece(this.type)){
            Coord possibleMoves[][]={{new Coord(-2,-1),new Coord(-2,1)},
                    {new Coord(-1,-2),new Coord(1,-2)},
                    {new Coord(-1,2),new Coord(1,2)},
                    {new Coord(2,-1),new Coord(2,1)}};
            Coord toCheck[]={new Coord(-1,0),new Coord(0,-1),new Coord(0,1),new Coord(1,0)};
            for(int i=0;i<toCheck.length;i++){
                if(this.c.add(toCheck[i]).isBetween(-1,-1,10,9)&&getPawnFromBoard(p,this.c.add(toCheck[i])).isEmpty()){
                    for(int j=0;j<possibleMoves[i].length;j++){
                        if(this.c.add(possibleMoves[i][j]).isBetween(-1,-1,10,9)&&!getPawnFromBoard(p,this.c.add(possibleMoves[i][j])).type.isColor(this.type)){
                            lc.add(this.c.add(possibleMoves[i][j]));
                        }
                    }
                }
            }
        }
        else if(Type.ELEPHANTR.isThispiece(this.type)){
            if(this.type.isRed()){
                if(this.c.add(2,2).isBetween(4,-1,10,9)&&!getPawnFromBoard(p,this.c.add(2,2)).type.isRed()&&getPawnFromBoard(p,this.c.add(1,1)).isEmpty()){
                    lc.add(this.c.add(2,2));
                }
                if(this.c.add(-2,-2).isBetween(4,-1,10,9)&&!getPawnFromBoard(p,this.c.add(-2,-2)).type.isRed()&&getPawnFromBoard(p,this.c.add(-1,-1)).isEmpty()){
                    lc.add(this.c.add(-2,-2));
                }
                if(this.c.add(-2,2).isBetween(4,-1,10,9)&&!getPawnFromBoard(p,this.c.add(-2,2)).type.isRed()&&getPawnFromBoard(p,this.c.add(-1,1)).isEmpty()){
                    lc.add(this.c.add(-2,2));
                }
                if(this.c.add(2,-2).isBetween(4,-1,10,9)&&!getPawnFromBoard(p,this.c.add(2,-2)).type.isRed()&&getPawnFromBoard(p,this.c.add(1,-1)).isEmpty()){
                    lc.add(this.c.add(2,-2));
                }
            }else {
                if(this.c.add(2,2).isBetween(-1, -1, 5, 9)&&!getPawnFromBoard(p,this.c.add(2,2)).type.isRed()&&getPawnFromBoard(p,this.c.add(1,1)).isEmpty()){
                    lc.add(this.c.add(2,2));
                }
                if(this.c.add(-2,-2).isBetween(-1, -1, 5, 9)&&!getPawnFromBoard(p,this.c.add(-2,-2)).type.isRed()&&getPawnFromBoard(p,this.c.add(-1,-1)).isEmpty()){
                    lc.add(this.c.add(-2,-2));
                }
                if(this.c.add(-2,2).isBetween(-1, -1, 5, 9)&&!getPawnFromBoard(p,this.c.add(-2,2)).type.isRed()&&getPawnFromBoard(p,this.c.add(-1,1)).isEmpty()){
                    lc.add(this.c.add(-2,2));
                }
                if(this.c.add(2,-2).isBetween(-1, -1, 5, 9)&&!getPawnFromBoard(p,this.c.add(2,-2)).type.isRed()&&getPawnFromBoard(p,this.c.add(1,-1)).isEmpty()){
                    lc.add(this.c.add(2,-2));
                }
            }
        }
        else if(Type.ADVISORR.isThispiece(this.type)){
            if(this.type.isRed()){
                if(this.c.add(1,1).isBetween(6,2,10,6)&&!getPawnFromBoard(p,this.c.add(1,1)).type.isRed()){
                    lc.add(this.c.add(1,1));
                }
                if(this.c.add(-1,-1).isBetween(6,2,10,6)&&!getPawnFromBoard(p,this.c.add(-1,-1)).type.isRed()){
                    lc.add(this.c.add(-1,-1));
                }
                if(this.c.add(-1,1).isBetween(6,2,10,6)&&!getPawnFromBoard(p,this.c.add(-1,1)).type.isRed()){
                    lc.add(this.c.add(-1,1));
                }
                if(this.c.add(1,-1).isBetween(6,2,10,6)&&!getPawnFromBoard(p,this.c.add(1,-1)).type.isRed()){
                    lc.add(this.c.add(1,-1));
                }
            }else {
                if (this.c.add(1, 1).isBetween(-1, 2, 3, 6) && !getPawnFromBoard(p, this.c.add(1, 1)).type.isBlack()) {
                    lc.add(this.c.add(1, 1));
                }
                if (this.c.add(1, -1).isBetween(-1, 2, 3, 6) && !getPawnFromBoard(p, this.c.add(1, -1)).type.isBlack()) {
                    lc.add(this.c.add(1, -1));
                }
                if (this.c.add(-1, 1).isBetween(-1, 2, 3, 6) && !getPawnFromBoard(p, this.c.add(-1, 1)).type.isBlack()) {
                    lc.add(this.c.add(-1, 1));
                }
                if (this.c.add(-1, -1).isBetween(-1, 2, 3, 6) && !getPawnFromBoard(p, this.c.add(-1, -1)).type.isBlack()) {
                    lc.add(this.c.add(-1, -1));
                }
            }

        }
        else if(Type.GENERALR.isThispiece(this.type)){
            if(this.type.isRed()){
                if(this.c.add(1,0).isBetween(6,2,10,6)&&!getPawnFromBoard(p,this.c.add(1,0)).type.isRed()){
                    lc.add(this.c.add(1,0));
                }
                if(this.c.add(0,-1).isBetween(6,2,10,6)&&!getPawnFromBoard(p,this.c.add(0,-1)).type.isRed()){
                    lc.add(this.c.add(0,-1));
                }
                if(this.c.add(0,1).isBetween(6,2,10,6)&&!getPawnFromBoard(p,this.c.add(0,1)).type.isRed()){
                    lc.add(this.c.add(0,1));
                }
                if(this.c.add(-1,0).isBetween(6,2,10,6)&&!getPawnFromBoard(p,this.c.add(-1,0)).type.isRed()){
                    lc.add(this.c.add(-1,0));
                }
            }else{
                if(this.c.add(1,0).isBetween(-1,2,3,6)&&!getPawnFromBoard(p,this.c.add(1,0)).type.isBlack()){
                    lc.add(this.c.add(1,0));
                }
                if(this.c.add(0,-1).isBetween(-1,2,3,6)&&!getPawnFromBoard(p,this.c.add(0,-1)).type.isBlack()){
                    lc.add(this.c.add(0,-1));
                }
                if(this.c.add(0,1).isBetween(-1,2,3,6)&&!getPawnFromBoard(p,this.c.add(0,1)).type.isBlack()){
                    lc.add(this.c.add(0,1));
                }
                if(this.c.add(-1,0).isBetween(-1,2,3,6)&&!getPawnFromBoard(p,this.c.add(-1,0)).type.isBlack()){
                    lc.add(this.c.add(-1,0));
                }

            }
        }
        return lc;
    }*/
}
