using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Tetris : MonoBehaviour {

    float fall = 0;
    private float fallspeed = 1;
    public bool allowrotate = true;
    public bool limitrotate = false;
    public AudioClip controll;
    public AudioClip land;
    private AudioSource AudioPlayer;
    private float continuousVerticalSpeed = 0.06f;
    private float continuousHorizontalSpeed = 0.1f;
    private float waittomove = 0.2f;
    private float VerticalTimer = 0;
    private float HorizontalTimer = 0;
    private bool VerticalMoveOne = false;
    private bool HorizontalMoveOne = false;
    private float buttondownV=0;
    private float buttondownH = 0;
    public bool iftwoplay = false;
    // Use this for initialization
    void Start () {
        AudioPlayer = GetComponent<AudioSource>();
        iftwoplay = FindObjectOfType<Gameone>().GetIfTwoPlayer();
        if (!iftwoplay)
        fallspeed = GameObject.Find("Grid_p1").GetComponent<Gameone>().fall_speed;
        else
        fallspeed = GameObject.Find("Grid_2").GetComponent<Gametwo>().fall_speed;
    }
	
	// Update is called once per frame
	void Update () {
        Controll();
        if (!iftwoplay)
            fallspeed = GameObject.Find("Grid_p1").GetComponent<Gameone>().fall_speed;
        else
            fallspeed = GameObject.Find("Grid_2").GetComponent<Gametwo>().fall_speed;
        UserInput();
    }
    private void UserInput()
    {
        if (Input.GetKeyDown(KeyCode.Slash))
        {

            do
            {
                transform.position += new Vector3(0, -1, 0);
            } while (CheckIsValidPosition());
            if (CheckIsValidPosition())
            {


            }
            else
            {
                transform.position = transform.position + new Vector3(0, 1, 0);
                enabled = false;
                FindObjectOfType<Gameone>().UpgradeGrid(this);
                AudioPlayer.PlayOneShot(land);
                FindObjectOfType<Gameone>().DeleteRow();
                //Debug.Log();
                if (FindObjectOfType<Gameone>().CheckIsAboverid(this))
                {
                    FindObjectOfType<Gameone>().GameOver();
                }
                FindObjectOfType<Gameone>().LandScoreAdd();
                FindObjectOfType<Gameone>().NextTetrisp1();

                tag = "Untagged";
            }

            fall = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            //Debug.Log("keyDown");
            FindObjectOfType<Gameone>().Minosave(this);
        }
         if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            rotate();

        }
    }
    private void Controll()
    {
        if(Input.GetKeyUp(KeyCode.LeftArrow)|| Input.GetKeyUp(KeyCode.RightArrow))
        {
            
            HorizontalMoveOne = false;
            buttondownH = 0;
            HorizontalTimer = 0;
        
        }
        if(Input.GetKeyUp(KeyCode.DownArrow))
        {
            VerticalMoveOne = false;
            buttondownV = 0;
            VerticalTimer = 0;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveleft();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveright();
        }
        else if (Input.GetKey(KeyCode.DownArrow) || Time.time - fall >= fallspeed)
        {
            movefall();
        }

    }
     public  bool CheckIsValidPosition()
    {
        
        foreach (Transform mino in transform)
        {

            Vector3 pos = FindObjectOfType<Gameone>().Round(mino.position);
           
            if (FindObjectOfType<Gameone>().EdgeDetect(pos) == false) //all thing in grid
            {
                return false;
            }
            
            if (FindObjectOfType<Gameone>().GetTransformGridPosition(pos)!=null)
            {
               // Debug.Log(FindObjectOfType<Gameone>().GetTransformGridPosition(pos).parent);
                return false;
            }

        }
        return true;
    }

    private void moveright()
    {
        if (HorizontalMoveOne)
        {
            if (buttondownH < waittomove)
            {
                buttondownH += Time.deltaTime;
            }
            if (HorizontalTimer < continuousHorizontalSpeed)
            {
                HorizontalTimer += Time.deltaTime;
                return;
            }
            HorizontalTimer = 0;
        }
        else
        {
            HorizontalMoveOne = true;
        }
        transform.position += new Vector3(1, 0, 0);

        if (CheckIsValidPosition())
        {

            AudioPlayer.PlayOneShot(controll);
        }
        else
        {
            transform.position = transform.position + new Vector3(-1, 0, 0);

        }
    }
    private void moveleft()
    {
        if (HorizontalMoveOne)
        {
            if (buttondownH < waittomove)
            {
                buttondownH += Time.deltaTime;
            }
            if (HorizontalTimer < continuousHorizontalSpeed)
            {
                HorizontalTimer += Time.deltaTime;
                return;
            }
            HorizontalTimer = 0;
        }
        else
        {
            HorizontalMoveOne = true;
        }
        transform.position += new Vector3(-1, 0, 0);
        if (CheckIsValidPosition())
        {

            AudioPlayer.PlayOneShot(controll);
        }
        else
        {
            transform.position = transform.position + new Vector3(1, 0, 0);

        }
    }
    private void movefall()
    {
        if (VerticalMoveOne)
        {
            if (buttondownV < waittomove)
            {
                buttondownV += Time.deltaTime;
            }
            if (VerticalTimer < continuousVerticalSpeed)
            {
                VerticalTimer += Time.deltaTime;
                return;
            }
            VerticalTimer = 0;
        }
        else
        {
            VerticalMoveOne = true;
        }
        transform.position += new Vector3(0, -1, 0);
        if (CheckIsValidPosition())
        {
            if (Input.GetKey(KeyCode.DownArrow))
                AudioPlayer.PlayOneShot(controll);
        }

        else
        {
            transform.position = transform.position + new Vector3(0, 1, 0);
            enabled = false;

            FindObjectOfType<Gameone>().UpgradeGrid(this);
            AudioPlayer.PlayOneShot(land);
            FindObjectOfType<Gameone>().DeleteRow();
            // Debug.Log(FindObjectOfType<Gameone>().CheckIsAboverid(this));
            if (FindObjectOfType<Gameone>().CheckIsAboverid(this))
            {
                FindObjectOfType<Gameone>().GameOver();
            }
            FindObjectOfType<Gameone>().LandScoreAdd();
            FindObjectOfType<Gameone>().NextTetrisp1();
            tag = "Untagged";

        }
        fall = Time.time;
    }
    private void rotate()
    {
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
                AudioPlayer.PlayOneShot(controll);
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
