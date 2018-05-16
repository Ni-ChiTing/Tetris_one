using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetris_demo : MonoBehaviour {

    float fall = 0;
    public float fallspeed = 0.001f;
    public bool allowrotate = true;
    public bool limitrotate = false;

    // Use this for initialization
    void Start () {
 
    }
	
	// Update is called once per frame
	void Update () {
        Controll();

    }
    
    private void Controll()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0,0);
            if (CheckIsValidPosition())
            {
              

            }
            else
            {
                transform.position = transform.position + new Vector3(1, 0, 0);

            }
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (CheckIsValidPosition())
            {


            }
            else
            {
                transform.position = transform.position + new Vector3(-1, 0, 0);

            }
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow)|| Time.time - fall >= fallspeed)
        {
            fall = Time.time;
            transform.position += new Vector3(0, -1, 0);
            if (CheckIsValidPosition())
            {

            }
            else
            {
                transform.position = transform.position + new Vector3(0, 1, 0);
                enabled = false;
                FindObjectOfType<Gamedemo>().UpgradeGrid(this);
                Debug.Log("--------------------------------------");
                FindObjectOfType<Gamedemo>().NextTetrisp1();
                if (FindObjectOfType<Gamedemo>().CheckIsAboverid(this))
                {
                    FindObjectOfType<Gamedemo>().clearall();
                }

            }
        }
        else  if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            fall = Time.time;
            if (allowrotate)
            {
                if (limitrotate)
                {
                    if (transform.rotation.eulerAngles.z >= 90)
                    {
                        transform.Rotate(0, 0, -90);
                    }
                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }
                }
                else
                {
                    transform.Rotate(0, 0, 90);
                }
                if (CheckIsValidPosition())
                {
                    
                }
                else
                {
                    if (limitrotate)
                    {
                        if (transform.rotation.eulerAngles.z >= 90)
                            transform.Rotate(0, 0, -90);
                        else
                            transform.Rotate(0, 0, 90);
                    }
                    else
                    {
                        transform.Rotate(0, 0, -90);
                    }

                }


            }
        }
    }
     public  bool CheckIsValidPosition()
    {
        foreach (Transform mino in transform)
        {
            Vector3 pos = FindObjectOfType<Gamedemo>().Round(mino.position);
            if (FindObjectOfType<Gamedemo>().EdgeDetect(pos) == false) //all thing in grid
            {
                return false;
            }
            
            if(FindObjectOfType<Gamedemo>().GetTransformGridPosition(pos)!=null)
            {
                return false;
            }

        }
        return true;
    }
}
