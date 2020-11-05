using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;

public static class Define
{
    public static readonly int MAX_NUM_OF_GAME = 2;  //鬼を入れ替える最大回数(これを超えても決着つかないと引き分け)
}

public class Timer : MonoBehaviourPunCallbacks
{
    float measure_time = 20;
    int num_of_game_count = Define.MAX_NUM_OF_GAME;
    int game_state=0;
    int player_number=0;
    bool is_first_loop = true;

    static readonly int SETTING_NEXT_OGRE = 0;
    static readonly int RUNNING_GAME_TIMER = 1;
    static readonly int STOPPING_GAME_TIMER = 2;
    static readonly int END_GAME = 3;

    GameObject Game_Center_Text;
    GameObject[] player = new GameObject[4];
    GameObject Network_Init;

    void Start()
    {
        Game_Center_Text = GameObject.Find("TimeManage_and_NetworkManage").transform.Find("Game_Center_Text").gameObject;
        Network_Init = GameObject.Find("TimeManage_and_NetworkManage");
    }


    void Update()
    {
        if(Network_Init.GetComponent<NetworkInit>().is_num_of_player_max)
        {
            if (is_first_loop)
            {
                for (int i = 0; i < 4; i++)
                {
                    string number = (i+1).ToString();
                    player[i] = GameObject.Find("Player" + number + "(Clone)");
                    player[i].transform.Find("Character1_Reference").transform.Find("Character1_Hips").transform.Find("Character1_RightUpLeg").transform.Find("Character1_RightLeg").transform.Find("Character1_RightFoot").transform.Find("Character1_RightToeBase").GetComponent<SphereCollider>().enabled = false;
                }
                is_first_loop = false;
            }
            
            if(game_state == SETTING_NEXT_OGRE)
            {
                player_number += 1;
                if (player_number > 3)
                {
                    player_number = 0;
                }
                if (player[player_number] == true)
                {
                    player[player_number].GetComponent<PlayerMove>().is_ogre = true;
                    player[player_number].transform.Find("HPUI").transform.Find("is_ogre").GetComponent<Text>().text = "鬼";
                    player[player_number].GetComponent<CapsuleCollider>().enabled = false;
                    Destroy(player[player_number].GetComponent<Rigidbody>());
                    game_state = 1;
                    measure_time = 20;
                }
            }
                    
            else if(game_state == RUNNING_GAME_TIMER)
            {
                measure_time -= Time.deltaTime; //スタートしてからの秒数を格納
                GetComponent<Text>().text = measure_time.ToString("F0");
                if (GameObject.Find("Sync_Variable_Manager(Clone)").GetComponent<SyncVariableManager>().player_exist_num <= 1)
                {
                    Game_Center_Text.GetComponent<Text>().text = PhotonNetwork.PlayerList[player_number].NickName + "の勝ち!";
                    GetComponent<Text>().text = "";
                    measure_time = 3;
                    game_state = 2;
                    num_of_game_count = 0;
                }
                else if (measure_time < 0)
                {
                    GetComponent<Text>().text = "";

                    measure_time = 3;
                    game_state = 2;
                    num_of_game_count -= 1;

                    if (num_of_game_count <= 0)
                    {
                        Game_Center_Text.GetComponent<Text>().text = "引き分け";
                    }
                    else
                    {
                        Game_Center_Text.GetComponent<Text>().text = "鬼の交代";
                    }

                    player[player_number].transform.Find("HPUI").transform.Find("is_ogre").GetComponent<Text>().text = "";
                    player[player_number].AddComponent<Rigidbody>();
                    player[player_number].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    player[player_number].GetComponent<PlayerMove>().is_ogre = false;
                    player[player_number].GetComponent<Rigidbody>().isKinematic = false;
                    player[player_number].GetComponent<CapsuleCollider>().enabled = true;

                }
            }

            else if (game_state == STOPPING_GAME_TIMER)
            {
                measure_time -= Time.deltaTime; //スタートしてからの秒数を格納
                if (measure_time < 0)
                {
                    Game_Center_Text.GetComponent<Text>().text = "";
                    if (num_of_game_count <= 0)
                    {
                        game_state = 3;
                    }
                    else
                    {
                        game_state = 0;
                    }
                }
            }
            
            else if (game_state == END_GAME)
            {
                PhotonNetwork.LeaveRoom();
                SceneManager.LoadScene("Start_Scene");

            }
        }
    }
}
