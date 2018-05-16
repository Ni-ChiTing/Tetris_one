using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour {

	// Use this for initialization
	void Start () {
        tag = "current_ghost";
        foreach(Transform mino in transform)
        {
            mino.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.2f);
        }
	}
	
	// Update is called once per frame
	void Update () {
        FollowActiveMino();

    }
    private void FollowActiveMino()
    {
        if(GameObject.FindGameObjectWithTag("currentactivemino")!=null)
        {
            transform.position = GameObject.FindGameObjectWithTag("currentactivemino").transform.position;
            transform.rotation = GameObject.FindGameObjectWithTag("currentactivemino").transform.rotation;
            movedown();
        }

    }
    private void movedown()
    {
        do
        {
            transform.position += new Vector3(0, -1, 0);
        } while (CheckIsValidPosition());
        if (CheckIsValidPosition())
        {
            transform.position = transform.position + new Vector3(0, 1, 0);

        }
        transform.position += new Vector3(0, 1, 0);
    }
    public bool CheckIsValidPosition()
    {

        foreach (Transform mino in transform)
        {

            Vector3 pos = FindObjectOfType<Gameone>().Round(mino.position);

            if (FindObjectOfType<Gameone>().EdgeDetect(pos) == false) //all thing in grid
            {
                return false;
            }

            if (FindObjectOfType<Gameone>().GetTransformGridPosition(pos) != null)
            {
                // Debug.Log(FindObjectOfType<Gameone>().GetTransformGridPosition(pos).parent);
                return false;
            }

        }
        return true;
    }
}
