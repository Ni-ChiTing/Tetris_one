using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class Gametwo : MonoBehaviour
{
    public GameObject g;
    private GameObject save;
    public static int gridwidth2 = 10;
    public static int gridheight2 = 20;
    public static Transform[,] grid2 = new Transform[gridwidth2, gridheight2];
    private List<string> minos = new List<string>();
    private static int size2 = 7;
    private static Tetris2 minotemp2 = null;
    public int onelinescore = 100;
    public int twolinescore = 300;
    public int threelinescore = 950;
    public int fourlinescore = 1920;
    private int linecount = 0;
    public Text score;
    public Text level;
    public Text lines;
    private static int current_score2 = 0;
    public AudioClip ClearSound;
    private AudioSource AudioPlayer;
    private Vector3 grid_pos;
    private Vector3 next_pos;
    private GameObject next;
    private GameObject previewnext;
    public GameObject nextblock;
    private bool Gamestart = false;
    public float fall_speed = 1f;
    public int currenLevel = 0;
    private int linesclear = 0;
    public int maxswap = 1;
    private int swaptime = 0;
    private GameObject ghost;
    // Use this for initialization
    void Start()
    {
        grid_pos = g.transform.position;
        next_pos = nextblock.transform.position;
        save = GameObject.Find("savemino_2");
        current_score2 = 0;

        // Debug.Log(save.transform.position);
        AudioPlayer = GetComponent<AudioSource>();
        InitMinoArray();
        NextTetrisp1();

    }

    // Update is called once per frame
    void Update()
    {
        ScoreUpdate();
        score.text = current_score2.ToString();
        UpdateLevel();
        UpdateSpeed();
    }

    private void UpdateLevel()
    {
        currenLevel = (int)(linesclear / 10);
        level.text = (currenLevel.ToString());
    }

    private void UpdateSpeed()
    {
        fall_speed = 1.0f - ((float)currenLevel * 0.05f);
    }

    public void ScoreUpdate()
    {

        if (linecount == 1)
        {
            current_score2 = current_score2 + onelinescore + currenLevel * 30;
            PlayLineCearSound();
        }
        else if (linecount == 2)
        {
            current_score2 = current_score2 + twolinescore + currenLevel * 60;
            PlayLineCearSound();
        }
        else if (linecount == 3)
        {
            current_score2 = current_score2 + threelinescore + currenLevel * 90;
            PlayLineCearSound();


        }
        else if (linecount == 4)
        {
            current_score2= current_score2 + fourlinescore + currenLevel * 120;
            PlayLineCearSound();
        }
        linesclear += linecount;
        lines.text = (linesclear.ToString());
        linecount = 0;
    }

    public void PlayLineCearSound()
    {
        AudioPlayer.PlayOneShot(ClearSound);
    }
    public void LandScoreAdd()
    {
        current_score2 = current_score2 + 15;
    }
    public void Minosave(Tetris2 tetris)
    {
        Vector3 block_pos = save.transform.position;
        Vector3 pos = new Vector3((float)(block_pos.x), (float)(block_pos.y + 2.5), 0);

        swaptime++;
        Debug.Log(swaptime);
        if (swaptime > maxswap)
        {
            return;
        }

        if (minotemp2 == null)
        {

            minotemp2 = tetris;
            minotemp2.transform.position = pos;
            minotemp2.enabled = false;
            minotemp2.tag = "currentsavemino2";
            NextTetrisp1();


        }
        else
        {
            Tetris2 t = minotemp2;
            minotemp2 = tetris;
            minotemp2.transform.position = pos;
            minotemp2.enabled = false;
            minotemp2.tag = "currentsavemino2";
            t.transform.position = new Vector3(Mathf.Round((gridwidth2 + 2 * (grid_pos.x)) / 2), (float)(gridheight2 + 1 + grid_pos.y), grid_pos.z); ;
            t.enabled = true;
            t.tag = "currentactivemino2";
            Destroy(GameObject.FindGameObjectWithTag("current_ghost2"));
            ghost = (GameObject)Instantiate(GameObject.FindGameObjectWithTag("currentactivemino2"), GameObject.FindGameObjectWithTag("currentactivemino2").transform.position, Quaternion.identity);
            Destroy(ghost.GetComponent<Tetris2>());
            ghost.AddComponent<Ghost2>();
        }
    }
    private void InitMinoArray()
    {
        minos.Clear();
        size2= 7;
        minos.Add("R/tetris_J2");
        minos.Add("R/tetris_I2");
        minos.Add("R/tetris_L2");
        minos.Add("R/tetris_O2");
        minos.Add("R/tetris_S2");
        minos.Add("R/tetris_T2");
        minos.Add("R/tetris_Z2");
    }
    public bool CheckIsAboverid(Tetris2 tetris)
    {
        for (int x = 0; x < gridwidth2; ++x)
        {
            foreach (Transform t in tetris.transform)
            {
                Vector3 pos = Round(t.position);
                if (pos.y > gridheight2 + grid_pos.y)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public bool IsRowFull(int y)
    {
        for (int x = 0; x < gridwidth2; ++x)
        {
            if (grid2[x, y] == null)
            {
                return false;
            }
        }
        linecount++;
        return true;
    }
    public void DeleteItem(int y)
    {
        for (int x = 0; x < gridwidth2; ++x)
        {
            Destroy(grid2[x, y].gameObject);
            grid2[x, y] = null;
        }
    }
    public void MoveDownRow(int y)
    {
        for (int x = 0; x < gridwidth2; x++)
        {
            grid2[x, y] = grid2[x, y + 1];
            if (grid2[x, y] != null)
                grid2[x, y].position += new Vector3(0, -1, 0);
            grid2[x, y + 1] = null;
        }

    }
    public void MoveAllRowDown(int y)
    {
        for (int i = y; i < gridheight2 - 1; ++i)
        {
            MoveDownRow(i);
        }
    }
    public void DeleteRow()
    {
        for (int y = 0; y < gridheight2; ++y)
        {
            if (IsRowFull(y))
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
        return ((int)pos.x >= (grid_pos.x + 1) && (int)pos.x <= gridwidth2 + (grid_pos.x) && (int)pos.y >= (grid_pos.y + 1));
    }
    public Vector3 Round(Vector3 pos)
    {
        return new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), pos.z);
    }
    public void NextTetrisp1()
    {
        Vector3 npos = new Vector3((float)(next_pos.x), (float)(next_pos.y + 2.5), 0);
        Vector3 pos = new Vector3(Mathf.Round((gridwidth2 + 2 * (grid_pos.x)) / 2), (float)(gridheight2 + 1 + grid_pos.y), grid_pos.z);
        if (!Gamestart)
        {
            Gamestart = true;

            next = (GameObject)Instantiate(Resources.Load(GetTetrisName(), typeof(GameObject)), pos, Quaternion.identity);
            previewnext = (GameObject)Instantiate(Resources.Load(GetTetrisName(), typeof(GameObject)), npos, Quaternion.identity);
            if (previewnext.ToString().Contains("tetris_I2"))
            {
                previewnext.transform.position = new Vector3((float)(next_pos.x - 0.5), (float)(next_pos.y + 3), 0);
            }
            else if (previewnext.ToString().Contains("tetris_O2"))
            {
                previewnext.transform.position = new Vector3((float)(next_pos.x - 0.5), (float)(next_pos.y + 2.5), 0);
            }
            previewnext.GetComponent<Tetris2>().enabled = false;
            next.tag = "currentactivemino2";
            ghostminospawn();
        }
        else
        {
            previewnext.transform.localPosition = pos;
            next = previewnext;
            next.GetComponent<Tetris2>().enabled = true;
            previewnext = (GameObject)Instantiate(Resources.Load(GetTetrisName(), typeof(GameObject)), npos, Quaternion.identity);
            if (previewnext.ToString().Contains("tetris_I2"))
            {
                previewnext.transform.position = new Vector3((float)(next_pos.x - 0.5), (float)(next_pos.y + 3), 0);
            }
            else if (previewnext.ToString().Contains("tetris_O2"))
            {
                previewnext.transform.position = new Vector3((float)(next_pos.x - 0.5), (float)(next_pos.y + 2.5), 0);
            }
            previewnext.GetComponent<Tetris2>().enabled = false;
            next.tag = "currentactivemino2";
            ghostminospawn();
        }
        swaptime = 0;
        //Debug.Log(next.transform.position);
    }
    public void ghostminospawn()
    {
        Destroy(GameObject.FindGameObjectWithTag("current_ghost2"));
        ghost = (GameObject)Instantiate(next, next.transform.position, Quaternion.identity);
        Destroy(ghost.GetComponent<Tetris2>());
        ghost.AddComponent<Ghost2>();
    }
    public void UpgradeGrid(Tetris2 tetris)
    {
        for (int y = 0; y < gridheight2; ++y)
        {
            for (int x = 0; x < gridwidth2; ++x)
            {
                if (grid2[x, y] != null)
                {
                    if (grid2[x, y].parent == tetris.transform)
                    {
                        grid2[x, y] = null;
                    }
                }
            }
        }
        foreach (Transform t in tetris.transform)
        {
            Vector2 pos = Round(t.position);
            //Debug.Log("grid_pos" + grid_pos + "  " + "pos" + pos);
            if (pos.y < gridheight2 + grid_pos.y + 1)
            {
                grid2[(int)(pos.x - grid_pos.x - 1), (int)(pos.y - grid_pos.y - 1)] = t;
            }
        }
    }
    public Transform GetTransformGridPosition(Vector3 p)
    {

        if (p.y > gridheight2 + grid_pos.y)
        {
            return null;
        }
        else
        {
            Vector3 pos = new Vector3(p.x - grid_pos.x - 1, p.y - grid_pos.y - 1, p.z);
            //Debug.Log("grid_pos" + grid_pos + "  " + "pos" + pos);
            return grid2[(int)pos.x, (int)pos.y];
        }
    }
    public void PrintGrid()
    {
        for (int y = 0; y < gridheight2; ++y)
        {
            for (int x = 0; x < gridwidth2; ++x)
            {
                if (grid2[x, y] != null)
                    Debug.Log("(" + x + "," + y + ")" + "  " + grid2[x, y]);
            }
        }
    }
    private string GetTetrisName()
    {
        if (size2 == 0)
        {
            minos.Clear();
            InitMinoArray();
            size2 = 7;
        }
        int randomnum = Random.Range(0, size2);
        size2--;
        string tetris_name = minos[randomnum];
        minos.RemoveAt(randomnum);
        return tetris_name;
    }
    public void GameOver()
    {
        gameovertwo.win = 0;
        Application.LoadLevel("gameovertwo");
    }
    public static int getscore()
    {
        return current_score2;
    }
}
