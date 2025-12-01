using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 计时
/// </summary>
public class Manager : MonoBehaviour
{
    public static Manager Instance;
    public GameObject[] lajimanage;
    private void Awake()
    {
        Instance = this;
        Nohandle();
    }

    public int score; //得分
    private bool isGameStart;
   // public GameObject Time_text;
   // public GameObject Score_text;
    public Text time_Txt;
    public Text score_Txt;
    private float gametime = 0;

    public Transform EndPanel;
    //private Transform ClickButton;
    public Transform WinPanel;
    public GameObject screen;

    private bool isGameOver;



    void Start()
    {
 
        //time_Txt = transform.Find("计时/time").gameObject.GetComponent<Text>();
        //score_Txt = transform.Find("得分/score").gameObject.GetComponent<Text>();
        //EndPanel = transform.Find("EndPanel").gameObject.GetComponent<Transform>();
        //WinPanel = transform.Find("WinPanel").gameObject.GetComponent<Transform>();
        // ClickButton = transform.Find("ClickButton").gameObject.GetComponent<Transform>();
    }


    void Update()
    {

        if (isGameOver) return;
        if (isGameStart)
        {
            gametime += Time.deltaTime;
            time_Txt.text = ((int)gametime).ToString() + "S";
            score_Txt.text = score.ToString();
           // Debug.Log(score);
            //Debug.Log(score_Txt.text);
            if (gametime > 40 && score < 30)
            {
                EndPanel.gameObject.SetActive(true);
                Nohandle();
                //ClickButton.gameObject.SetActive(true);
                isGameOver = true;
            }
            if (gametime < 40 && score > 30)
            {
                WinPanel.gameObject.SetActive(true);
                Nohandle();
                isGameOver = true;
            }
        }

    }
    /// <summary>
    /// 游戏开始
    /// </summary>
    public  void StartGame()
    {
        screen.gameObject.SetActive(false);
       // Time_text.SetActive(true);
       // Score_text.SetActive(true);
        isGameStart = true;
         Yeshandle();
    }
    public void ReStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    /// <summary>
    /// 不能拿起垃圾
    /// </summary>
    public void Nohandle()
    {
        for (int i = 0; i < lajimanage.Length; i++)
        {
            if (lajimanage[i]!=null)
            {
            lajimanage[i].gameObject.GetComponent<Rigidbody>().useGravity = false;
            lajimanage[i].gameObject.GetComponent<BoxCollider>().enabled = false;

            }

        }
    }
    /// <summary>
    /// 可以拿起垃圾
    /// </summary>
    public void Yeshandle()
    {
        for (int i = 0; i < lajimanage.Length; i++)
        {
            lajimanage[i].gameObject.GetComponent<Rigidbody>().useGravity = true;
            lajimanage[i].gameObject.GetComponent<BoxCollider>().enabled = true;

        }
    }

}
