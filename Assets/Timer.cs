using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class Timer : MonoBehaviour
{
    float countTime = 20;
    float endGame_waitTime = 2;
    int isGameEnd=0;

    public GameObject GameEnd_text;
    void Start()
    {
    }
    
    void Update()
    {
        switch(isGameEnd)
        {
            case 0:
                countTime -= Time.deltaTime; //スタートしてからの秒数を格納
                GetComponent<Text>().text = countTime.ToString("F0"); //小数2桁にして表示
                if (countTime < 0)
                {
                    GetComponent<Text>().text = "";
                    GameEnd_text.GetComponent<Text>().text = "ゲーム終了";
                    countTime = 3;
                    isGameEnd = 1;
                }
                break;
            case 1:
                countTime -= Time.deltaTime; //スタートしてからの秒数を格納
                if (countTime < 0)
                {
                    isGameEnd = 2;
                }
                break;
            case 2:
                SceneManager.LoadScene("Start_Scene");
                break;

            default:
                break;
        }
        
        if (countTime < 0)
        {
            
        }
            
    }
}
