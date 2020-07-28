using System.Collections;
using System.Collections.Generic;
using Unity.UIWidgets.widgets;
using UnityEditor;
using UnityEngine;

public class foroneblock : MonoBehaviour
{
    
    //public Vector3 rotationPoint;
    public static int height = 24;
    public static int width = 10;
    //标识空间是否被方块占用
    public static UnityEngine.Transform[,] bitmap = new UnityEngine.Transform[height, width];
    public GameObject startLocation;
    public bool gameOver;
    private float pretime;
    public float falltime = 0.8f;
    void Start()
    {
        pretime = Time.time;
        gameOver = false;
        startLocation = GameObject.FindGameObjectWithTag("StartLocation");
        if (!moveValid())
        {
            gameOver = true;
            startLocation.SendMessage("setGame");
        }
            
    }

    // Update is called once per frame
    void Update()
    {
        if(!gameOver)
        {
            if(Input.GetKeyDown(KeyCode.LeftArrow))
            {
                transform.position += new Vector3(-1,0,0);
                if(!moveValid())
                    transform.position -= new Vector3(-1, 0, 0);
            }
            if(Input.GetKeyDown(KeyCode.RightArrow))
            {
                transform.position += new Vector3(1, 0, 0);
                if(!moveValid())
                    transform.position -= new Vector3(1, 0, 0);
            }
            if(Input.GetKeyDown(KeyCode.UpArrow))
            {
                transform.RotateAround(transform.TransformPoint(Vector3.zero),Vector3.forward,-90);
                if(!moveValid())
                    transform.RotateAround(transform.TransformPoint(Vector3.zero), Vector3.forward, 90);
            }

            if(Input.GetKeyDown(KeyCode.DownArrow) || (Time.time-pretime>falltime))
            {
                transform.position += new Vector3(0,-1,0);
                if(!moveValid())
                {
                    transform.position -= new Vector3(0, -1, 0);
                    addbit();
                    checkFullLine();
                    startLocation.SendMessage("newAblock");
                    this.enabled = false;
                }
                pretime = Time.time;
            }
        }
    }
    //检查是否有满的一行
    void checkFullLine()
    {
        for(int row = 0;row<height;++row)
        {
            if(checkLine(row))
            {
                startLocation.SendMessage("addGrade");
                deleteLine(row);//删除该行
                moveLines(row);//移动上面的方块
                --row;
            }
        }
    }
    void moveLines(int row)
    {
        for(;row<height-1;++row)
        {
            for(int col=0;col<width;++col)
            {
                if (bitmap[row+1, col] != null)
                {
                    bitmap[row, col] = bitmap[row + 1, col];
                    bitmap[row, col].transform.position += new Vector3(0,-1,0);
                    bitmap[row + 1, col] = null;
                }
                
            }
        }
    }
    void deleteLine(int row)
    {
        for(int col=0;col<width;++col)
        {
            Destroy(bitmap[row,col].gameObject);
            bitmap[row, col] = null;
        }
    }
    bool checkLine(int row)
    {
        for(int col=0;col<width;++col)
        {
            if (bitmap[row, col] == null)
            {
                return false;
            }
        }
        return true;
    }
    //根据方块的位置，设置标志
    void addbit()
    {
        foreach (UnityEngine.Transform child in transform)
        {
            //int x = Mathf.RoundToInt(child.transform.position.x);
            int col = Mathf.RoundToInt(child.transform.position.x);
            int row = Mathf.RoundToInt(child.transform.position.y);
            bitmap[row, col] = child;
        }
    }
    //移动的有效性
    bool moveValid()
    {
        foreach(UnityEngine.Transform child in transform)
        {
            //int x = Mathf.RoundToInt(child.transform.position.x);
            int col = Mathf.RoundToInt(child.position.x);
            int row = Mathf.RoundToInt(child.position.y);
            if(row>=0&&row<height&&col>=0&&col<width)
            {
                if (bitmap[row, col] != null)
                    return false;
            }
            else 
            {
                return false;
            }
        }
        return true;
    }

    //删除此方块
    void selfDelete()
    {
        foreach (UnityEngine.Transform child in transform)
        {
            //int x = Mathf.RoundToInt(child.transform.position.x);
            int col = Mathf.RoundToInt(child.position.x);
            int row = Mathf.RoundToInt(child.position.y);
            //Destroy(child.gameObject);
            bitmap[row, col] = null;
        }
        Destroy(gameObject);
    }
}
