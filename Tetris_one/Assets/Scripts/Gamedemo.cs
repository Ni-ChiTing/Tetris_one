using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Gamedemo : MonoBehaviour {
    private GameObject g;
    public static int gridwidth = 20;
    public static int gridheight = 20;
    public static int count = 0;
    public static Transform[,] grid =new Transform[gridwidth,gridheight];
    private int tetriscount = 0;
    public Text Timetext;
    float timestart = 0;
    float timeend = 0;
 
    // Use this for initialization
    void Start () {
        g = GameObject.Find("Grid_p1");
        NextTetrisp1();
        while(tetriscount<2000)
        {
            TetrisInStart();
            tetriscount++;
        }
        timestart = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log(count);
        if(count-tetriscount<41)
        {
            timeend = Time.time;
            float a = timeend - timestart;
            SetTimeText(a.ToString());
        }

    }
    public void SetTimeText(string s)
    {
        Timetext.text = s;
    }
    public void clearall()
    {
        for (int y = 0; y < gridheight; ++y)
        {
            for (int x = 0; x < gridwidth; ++x)
            {
                if (grid[x, y] != null)
                {
                    grid[x,y].GetComponent<SpriteRenderer>().color= new Color(1f, 1f, 1f, 0.0f);
                    grid[x, y] = null;
                }
            }
        }
    }
        
    
    public bool EdgeDetect(Vector3 pos)
    {
        Vector3 grid_pos = g.transform.localPosition;
        return ((int)pos.x >= (grid_pos.x + 1) && (int)pos.x <= gridwidth + (grid_pos.x) && (int)pos.y >= (grid_pos.y + 1));
    }
    public Vector3 Round(Vector3 pos)
    {
        return new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y),pos.z);
    }
    public void NextTetrisp1()
    {
        Vector3 grid_pos = g.transform.position;
        Vector3 pos = new Vector3(Mathf.Round(grid_pos.x+1+(count%20)), (float)(gridheight + 1 + grid_pos.y), grid_pos.z);
        ++count;
        GameObject next = (GameObject)Instantiate(Resources.Load(GetTetrisName(), typeof(GameObject)), pos, Quaternion.identity);
        //Debug.Log(next.transform.position);
    }
    private void TetrisInStart ()
    {
        Vector3 grid_pos = g.transform.position;
        Vector3 pos = new Vector3(Mathf.Round(grid_pos.x + 1 + (count % 20)), (float)(gridheight + 1 + grid_pos.y), grid_pos.z);
        ++count;
        GameObject next = (GameObject)Instantiate(Resources.Load(GetTetrisName(), typeof(GameObject)), pos, Quaternion.identity);
        next.GetComponent<Tetris_demo>().enabled = false;
        //Debug.Log(next.transform.position);
    }
    public void UpgradeGrid(Tetris_demo tetris)
    {
        for (int y = 0; y < gridheight; ++y)
        {
            for (int x = 0; x < gridwidth; ++x)
            {
                if (grid[x, y] != null)
                {
                    if (grid[x, y].parent == tetris.transform)
                    {
                        grid[x, y] = null;
                    }
                }
            }
        }
        foreach (Transform t in tetris.transform)
        {
            Vector2 pos = Round(t.position);
            Vector3 grid_pos = g.transform.localPosition;
            //Debug.Log("grid_pos" + grid_pos + "  " + "pos" + pos);
            if (pos.y < gridheight + grid_pos.y + 1)
            {
                grid[(int)(pos.x - grid_pos.x - 1), (int)(pos.y - grid_pos.y - 1)] = t;
            }
        }
    }
    public Transform GetTransformGridPosition(Vector3 p)
    {
        Vector3 grid_pos = g.transform.localPosition;

        if (p.y > gridheight + grid_pos.y)
        {
            return null;
        }
        else
        {
            Vector3 pos = new Vector3(p.x - grid_pos.x - 1, p.y - grid_pos.y - 1,p.z);
            //Debug.Log("grid_pos" + grid_pos + "  " + "pos" + pos);
            return grid[(int)pos.x, (int)pos.y];
        }
    }
    public void PrintGrid()
    {
        for (int y = 0; y < gridheight; ++y)
        {
            for (int x = 0; x < gridwidth; ++x)
            {
                if(grid[x,y]!=null)
                Debug.Log("("+x+","+y+")"+"  "+grid[x, y]);
            }
        }
    }
    private string GetTetrisName()
    {
        int randomnum = Random.Range(1, 8);
        string tetris_name = "R/tetris1";
        switch (randomnum)
        {
            case 1:
                tetris_name = "R/tetris2";
                break;
            case 2:
                tetris_name = "R/tetris3";
                break;
            case 3:
                tetris_name = "R/tetris4";
                break;
            case 4:
                tetris_name = "R/tetris5";
                break;
            case 5:
                tetris_name = "R/tetris6";
                break;
            case 6:
                tetris_name = "R/tetris7";
                break;
            case 7:
                tetris_name = "R/tetris1";
                break;


        }
        return tetris_name;
    }
    public bool CheckIsAboverid(Tetris_demo tetris)
    {
        Vector3 grid_pos = g.transform.localPosition;
        for (int x = 0; x < gridwidth; ++x)
        {
            foreach (Transform t in tetris.transform)
            {
                Vector3 pos = Round(t.position);
                if (pos.y > gridheight + grid_pos.y)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
