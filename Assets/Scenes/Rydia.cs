using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rydia : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
		Grid[] grid=GameObject.FindObjectsOfType( typeof(Grid) ) as Grid[];
		SpriteRenderer s=GetComponent<SpriteRenderer>();
		Debug.Log(""+s.bounds.size.x);
		Vector3 nv=new Vector3(GetComponent<Transform>().localScale.x*(grid[0].cellSize.x/s.bounds.size.x),GetComponent<Transform>().localScale.y*(grid[0].cellSize.x/s.bounds.size.x),1);
		GetComponent<Transform>().localScale=nv;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
