using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.DocZh.Components;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class start : MonoBehaviour
{
    // Start is called before the first frame update
    private bool gameover;
    public Texture texture;
    public Rect buttonarea;
    public Rect gradearea;
    public Rect overarea;
    public int grade;
    public GameObject[] toDelete;
    public GameObject[] Blocks;
    //public Texture play;
    //public GUIContent mycontent;
    public GUIStyle mystyle;
    void Start()
    {
        gameover = true;
        grade = 0;
    }

    void OnGUI()
    {
        //开始按钮
        bool bt = GUI.Button(buttonarea,"Play");
        //点击开始按钮，清理游戏界面，重新开始
        if (bt)
        {
            UnityEngine.Debug.Log("button hit");
            if (gameover)
            {
                grade = 0;
                gameover = false;
                toDelete = GameObject.FindGameObjectsWithTag("Blocks");
                foreach(GameObject child in toDelete)
                {
                    child.SendMessage("selfDelete");
                }
                newAblock();
            }
        }
        //分数显示
        if(gameover)
        {
            GUI.Label(overarea, texture);
        }
        GUI.Label(gradearea,"得分："+grade,mystyle);

    }
    //产生一个新的Block
    void addGrade()
    {
        ++grade;
    }
    void newAblock()
    {
        Instantiate(Blocks[Random.Range(0,Blocks.Length)],transform.position,Quaternion.identity);
    }

    void setGame()
    {
        gameover = true;
    }

}
