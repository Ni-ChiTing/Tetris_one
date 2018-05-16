using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class Gameone : MonoBehaviour {
    public GameObject g;
    private GameObject save;
    public static int gridwidth = 10;
    public static int gridheight = 20;
    public static Transform[,] grid =new Transform[gridwidth,gridheight];
    private List<string> minos = new List<string>();
    private static int size = 7;
    private static Tetris minotemp=null;
    public int onelinescore = 100;
    public int twolinescore = 300;
    public int threelinescore = 950;
    public int fourlinescore = 1920;
    private int linecount = 0;
    public Text score;
    public Text level;
    public Text lines;
    private static int current_score=0;
    public AudioClip ClearSound;
    private AudioSource AudioPlayer;
    private Vector3 grid_pos;
    private Vector3 next_pos;
    private GameObject next;
    private GameObject previewnext;
    public GameObject nextblock;
    private bool Gamestart = false;
    public float fall_speed=1.0f;
    public int currenLevel = 0;
    private int linesclear = 0;
    public int maxswap = 1;
    private int swaptime = 0;
    private GameObject ghost;
    public bool iftwoplay = false;
    // Use this for initialization
    void Start () {
        grid_pos = g.transform.position;
        next_pos= nextblock.transform.position;
        save = GameObject.Find("savemino");
        current_score = 0;
        
       // Debug.Log(save.transform.position);
        AudioPlayer = GetComponent<AudioSource>();
        InitMinoArray();
        NextTetrisp1();
            
    }
	public bool GetIfTwoPlayer()
    {
        return iftwoplay;
    }
	// Update is called once per frame
	void Update () {
        ScoreUpdate();
        score.text = current_score.ToString();
        UpdateLevel();
        UpdateSpeed();
    }

    private void UpdateLevel()
    {
        currenLevel = (int)(linesclear / 10);
        level.text=(currenLevel.ToString());
    }

    private void UpdateSpeed()
    {
        fall_speed = 1.0f-((float)currenLevel * 0.05f);
    }

    public void ScoreUpdate()
    {

        if(linecount==1)
        {
            current_score = current_score + onelinescore+currenLevel*30;
            PlayLineCearSound();
        }
        else if(linecount==2)
        {
            current_score = current_score + twolinescore+ currenLevel * 60;
            PlayLineCearSound();
        }
        else if(linecount==3)
        {
            current_score = current_score + threelinescore+ currenLevel * 90;
            PlayLineCearSound();


        }
        else if(linecount==4)
        {
            current_score = current_score + fourlinescore+currenLevel * 120;
            PlayLineCearSound();
        }
        linesclear += linecount;
        lines.text = (linesclear.ToString());
        linecount =0;
    } 
    
    public void PlayLineCearSound()
    {
        AudioPlayer.PlayOneShot(ClearSound);
    }
    public void LandScoreAdd()
    {
        current_score = current_score + 15;
    }
    public void Minosave(Tetris tetris)
    {
        Vector3 block_pos = save.transform.position;
        Vector3 pos = new Vector3((float)(block_pos.x), (float)(block_pos.y + 2.5), 0);

        swaptime++;
        Debug.Log(swaptime);
        if(swaptime>maxswap)
        {
            return;
        }

        if (minotemp==null)
        {
            
            minotemp = tetris;
            minotemp.transform.position = pos;
            minotemp.enabled = false;
            minotemp.tag = "currentsavemino";
            NextTetrisp1();

        }
        else
        {
            Tetris t = minotemp;
            minotemp = tetris;
            minotemp.transform.position = pos;
            minotemp.enabled = false;
            minotemp.tag = "currentsavemino";
            t.transform.position= new Vector3(Mathf.Round((gridwidth + 2 * (grid_pos.x)) / 2), (float)(gridheight + 1 + grid_pos.y), grid_pos.z); ;
            t.enabled = true;
            t.tag= "currentactivemino";
            Destroy(GameObject.FindGameObjectWithTag("current_ghost"));
            ghost = (GameObject)Instantiate(GameObject.FindGameObjectWithTag("currentactivemino"), GameObject.FindGameObjectWithTag("currentactivemino").transform.position, Quaternion.identity);
            Destroy(ghost.GetComponent<Tetris>());
            ghost.AddComponent<Ghost>();
        }
    }
    private void InitMinoArray()
    {
        minos.Clear();
        size = 7;
        minos.Add("R/tetris_J");
        minos.Add("R/tetris_I");
        minos.Add("R/tetris_L");
        minos.Add("R/tetris_O");
        minos.Add("R/tetris_S");
        minos.Add("R/tetris_T");
        minos.Add("R/tetris_Z");
    }
    public bool CheckIsAboverid (Tetris tetris)
    {
        for(int x=0;x<gridwidth;++x)
        {
            foreach(Transform t in tetris.transform)
            {
                Vector3 pos = Round(t.position);
                if (pos.y>gridheight+grid_pos.y)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool IsRowFull(int y)
    {
        for(int x=0; x<gridwidth;++x)
        {
            if(grid[x,y]==null)
            {
                return false;
            }
        }
        linecount++;
        return true;
    }
    public void DeleteItem(int y)
    {
        for( int x=0;x<gridwidth;++x)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }
    public void MoveDownRow(int y)
    {
            for(int x=0;x<gridwidth;x++)
            {
                grid[x, y] = grid[x, y + 1];
                if(grid[x, y]!=null)
                grid[x, y].position += new Vector3(0, -1, 0);
                grid[x, y + 1] = null;
            }
        
    }
    public void MoveAllRowDown(int y)
    {
        for(int i = y;i<gridheight-1;++i)
        {
            MoveDownRow(i);
        }
    }
    public void DeleteRow()
    {
        for(int y=0;y<gridheight;++y)
        {
            if(IsRowFull(y))
            {
                DeleteItem(y);
                MoveAllRowDown(y);
                y = y - 1;
            }
        }
    }
    public bool EdgeDetect(Vector3 pos)
    {
        //Debug.Log(g);
        return ((int)pos.x >= (grid_pos.x + 1) && (int)pos.x <= gridwidth + (grid_pos.x) && (int)pos.y >= (grid_pos.y + 1));
    }
    public Vector3 Round(Vector3 pos)
    {
        return new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y),pos.z);
    }
    public void NextTetrisp1()
    {
        Vector3 npos = new Vector3((float)(next_pos.x), (float)(next_pos.y + 2.5), 0);
        Vector3 pos = new Vector3(Mathf.Round((gridwidth + 2 * (grid_pos.x)) / 2), (float)(gridheight + 1 + grid_pos.y), grid_pos.z);
        if (!Gamestart)
        {
            Gamestart = true;
   
            next = (GameObject)Instantiate(Resources.Load(GetTetrisName(), typeof(GameObject)), pos, Quaternion.identity);
            previewnext= (GameObject)Instantiate(Resources.Load(GetTetrisName(), typeof(GameObject)), npos, Quaternion.identity);
            if(previewnext.ToString().Contains("tetris_I"))
            {
                previewnext.transform.position= new Vector3((float)(next_pos.x-0.5), (float)(next_pos.y + 3), 0);
            }
            else if (previewnext.ToString().Contains("tetris_O"))
            {
                previewnext.transform.position = new Vector3((float)(next_pos.x - 0.5), (float)(next_pos.y + 2.5), 0);
            }
            previewnext.GetComponent<Tetris>().enabled = false;
            next.tag = "currentactivemino";
            ghostminospawn();
        }
        else
        {
            previewnext.transform.localPosition = pos;
            next = previewnext;
            next.GetComponent<Tetris>().enabled = true;
            previewnext = (GameObject)Instantiate(Resources.Load(GetTetrisName(), typeof(GameObject)), npos, Quaternion.identity);
            if (previewnext.ToString().Contains("tetris_I"))
            {
                previewnext.transform.position = new Vector3((float)(next_pos.x - 0.5), (float)(next_pos.y + 3), 0);
            }
            else if (previewnext.ToString().Contains("tetris_O"))
            {
                previewnext.transform.position = new Vector3((float)(next_pos.x-0.5), (float)(next_pos.y + 2.5), 0);
            }
            previewnext.GetComponent<Tetris>().enabled = false;
            next.tag = "currentactivemino";
            ghostminospawn();
        }
        swaptime = 0;
        //Debug.Log(next.transform.position);
    }
    public void ghostminospawn()
    {
        Destroy(GameObject.FindGameObjectWithTag("current_ghost"));
        ghost = (GameObject)Instantiate(next,next.transform.position, Quaternion.identity);
        Destroy(ghost.GetComponent<Tetris>());
        ghost.AddComponent<Ghost>();
    }
    public void UpgradeGrid(Tetris tetris)
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
            //Debug.Log("grid_pos" + grid_pos + "  " + "pos" + pos);
            if (pos.y < gridheight + grid_pos.y + 1)
            {
                grid[(int)(pos.x - grid_pos.x - 1), (int)(pos.y - grid_pos.y - 1)] = t;
            }
        }
    }
    public Transform GetTransformGridPosition(Vector3 p)
    {

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
        if(size==0)
        {
            minos.Clear();
            InitMinoArray();
            size = 7;
        }
        int randomnum = Random.Range(0, size);
        size--;
        string tetris_name = minos[randomnum];
        minos.RemoveAt(randomnum);
        return tetris_name;
    }
    public void GameOver()
    {
       
        if(!iftwoplay)
            Application.LoadLevel("Gameover");
        else
        {
            gameovertwo.win = 1;
            Application.LoadLevel("gameovertwo");
        }
            
    }
    public static int getscore()
    {

        return current_score;
    }
}
