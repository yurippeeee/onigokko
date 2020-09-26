using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;


public class Timer : MonoBehaviour
{
    float countTime = 10;
    int switch_count = 2;
    int isGameEnd=2;
    int random;

    public int player_exit_num = 4;
    public GameObject GameEnd_text;
    GameObject[] player = new GameObject[4];
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            string s = i.ToString();
            player[i] = GameObject.Find("Player"+s);
        }
    }
    
    void Update()
    {
        switch(isGameEnd)
        {
            case 0:
                countTime -= Time.deltaTime; //スタートしてからの秒数を格納
                GetComponent<Text>().text = countTime.ToString("F0");
                if (player_exit_num <= 1)
                {
                    GameEnd_text.GetComponent<Text>().text = "Player"+ random.ToString()+"の勝ち";
                    GetComponent<Text>().text = "";
                    countTime = 3;
                    isGameEnd = 1;
                    switch_count -= 1;
                }
                else if (countTime < 0)
                {
                    GetComponent<Text>().text = "";
                    
                    countTime = 3;
                    isGameEnd = 1;
                    switch_count -= 1;

                    if (switch_count <= 0)
                    {
                        GameEnd_text.GetComponent<Text>().text = "引き分け";
                    }
                    else
                    {
                        GameEnd_text.GetComponent<Text>().text = "鬼の交代";
                    }

                    player[random].AddComponent<Rigidbody>();
                    player[random].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                    player[random].GetComponent<PlayerMove>().ogre = false;
                    player[random].GetComponent<Rigidbody>().isKinematic = false;
                }
                break;
            case 1:
                countTime -= Time.deltaTime; //スタートしてからの秒数を格納
                if (countTime < 0)
                {
                    GameEnd_text.GetComponent<Text>().text = "";
                    if (switch_count <= 0)
                    {
                        isGameEnd = 3;
                    }
                    else
                    {
                        isGameEnd = 2;
                    }
                }
                break;
            case 2:
                while(player[random = Random.Range(0, 4)] == false){}
                player[random].GetComponent<PlayerMove>().ogre = true;
                Destroy(player[random].GetComponent<Rigidbody>());
                isGameEnd = 0;
                countTime = 10;
                break;
            case 3:
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
