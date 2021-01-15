using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Data;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;

public class Timer : MonoBehaviourPunCallbacks
{
    static readonly int SETTING_NEXT_OGRE = 0;
    static readonly int RUNNING_GAME_TIMER = 1;
    static readonly int STOPPING_GAME_TIMER = 2;
    static readonly int END_GAME = 3;

    public float game_time = 10;
    public float measure_time = 10;
    public int num_of_game_count = 4;
    int game_state=0;
    int player_number=0;
    bool is_first_loop = true;

    GameObject game_center_text;
    GameObject[] player = new GameObject[4];
    GameObject[] player_name = new GameObject[4];
    GameObject network_init;

    public DataTable player_rank_table = new DataTable();


    void Start()
    {
        game_center_text = GameObject.Find("TimeManage_and_NetworkManage").transform.Find("Game_Center_Text").gameObject;
        network_init = GameObject.Find("TimeManage_and_NetworkManage");

        player_rank_table.Columns.Add("Player_Name", Type.GetType("System.String"));
        player_rank_table.Columns.Add("HP", Type.GetType("System.Int32"));
    }


    void Update()
    {
        if(network_init.GetComponent<NetworkInit>().is_num_of_player_max)
        {
            if (is_first_loop)
            {
                for (int i = 0; i < 4; i++)
                {
                    string number = (i+1).ToString();
                    player[i] = GameObject.Find("Player" + number + "(Clone)");
                    player_name[i] = GameObject.Find("TimeManage_and_NetworkManage").transform.Find("PlayerName" + number + "(Clone)").gameObject;
                    player[i].transform.Find("Character1_Reference").transform.Find("Character1_Hips").transform.Find("Character1_RightUpLeg").transform.Find("Character1_RightLeg").transform.Find("Character1_RightFoot").transform.Find("Character1_RightToeBase").GetComponent<SphereCollider>().enabled = false;
                    player_rank_table.Rows.Add(player[i].transform.Find("HPUI").transform.Find("PlayerName").GetComponent<Text>().text, 100);
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
                    player_name[player_number].transform.Find("is_ogre").GetComponent<Text>().text = "鬼";
                    player[player_number].GetComponent<CapsuleCollider>().enabled = false;
                    Destroy(player[player_number].GetComponent<Rigidbody>());
                    game_state = 1;
                    measure_time = game_time;
                }
                GameObject.Find("Sync_Variable_Manager(Clone)").GetComponent<SyncVariableManager>().timer_start_sync = false;
            }
                    
            else if(game_state == RUNNING_GAME_TIMER)
            {
                measure_time -= Time.deltaTime; //スタートしてからの秒数を格納
                GetComponent<Text>().text = measure_time.ToString("F0");

                if (GameObject.Find("Sync_Variable_Manager(Clone)").GetComponent<SyncVariableManager>().player_exist_num <= 1)
                {
                    DataRow[] dRows = player_rank_table.Select("", "HP DESC");
                    int j = 1;
                    for (int i = 0; i < 4; i++)
                    {
                        if ((i != 0) && ((int)dRows[i]["HP"] == (int)dRows[i - 1]["HP"]))
                        {
                            j--;
                        }
                        
                        GameObject.Find("TimeManage_and_NetworkManage").transform.Find("Rank" + (i + 1).ToString()).GetComponent<Text>().text = j.ToString() + "位　" + dRows[i]["Player_Name"];
                        if (j == 1)
                        {
                            GameObject.Find("TimeManage_and_NetworkManage").transform.Find("Rank" + (i + 1).ToString()).GetComponent<Text>().color = Color.yellow;
                        }

                        j++;
                    }

                    GameObject.Find("Player" + NetworkInit.player_num.ToString() + "(Clone)").GetComponent<PlayerMove>().enabled = false;
                    GameObject.Find("Item(Clone)").GetComponent<ItemCatch>().enabled = false;
                    //game_center_text.GetComponent<Text>().text = PhotonNetwork.PlayerList[player_number].NickName + "の勝ち!";
                    GetComponent<Text>().text = "";
                    measure_time = 3;
                    game_state = 2;
                    num_of_game_count = 0;
                    
                }

                else if ((measure_time < 0) || GameObject.Find("Sync_Variable_Manager(Clone)").GetComponent<SyncVariableManager>().timer_start_sync)
                {
                    GetComponent<Text>().text = "";

                    measure_time = 3;
                    game_state = 2;
                    num_of_game_count -= 1;

                    GameObject.Find("Item(Clone)").GetComponent<ItemCatch>().timer_end = true;

                    if (num_of_game_count <= 0)
                    {
                        DataRow[] dRows = player_rank_table.Select("","HP DESC");
                        int j = 1;
                        for (int i = 0; i < 4; i++)
                        {
                            if ((i != 0) && ((int)dRows[i]["HP"] == (int)dRows[i - 1]["HP"]) )
                            {
                                j--;
                            }
                            GameObject.Find("TimeManage_and_NetworkManage").transform.Find("Rank" + (i + 1).ToString()).GetComponent<Text>().text = j.ToString() + "位　" + dRows[i]["Player_Name"];
                            if (j == 1)
                            {
                                GameObject.Find("TimeManage_and_NetworkManage").transform.Find("Rank" + (i + 1).ToString()).GetComponent<Text>().color = Color.yellow;
                            }

                            j++;
                        }
                    }
                    else
                    {
                        game_center_text.GetComponent<Text>().text = "鬼の交代";
                    }

                    if(GameObject.Find("Player" + NetworkInit.player_num.ToString() + "(Clone)"))
                    {
                        GameObject.Find("Player" + NetworkInit.player_num.ToString() + "(Clone)").GetComponent<Animator>().SetBool("Run", false);
                        GameObject.Find("Player" + NetworkInit.player_num.ToString() + "(Clone)").GetComponent<PlayerMove>().enabled = false;
                    }

                    player[player_number].transform.Find("HPUI").transform.Find("is_ogre").GetComponent<Text>().text = "";
                    player_name[player_number].transform.Find("is_ogre").GetComponent<Text>().text = "";
                    player[player_number].AddComponent<Rigidbody>();
                    player[player_number].GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    player[player_number].GetComponent<PlayerMove>().is_ogre = false;
                    player[player_number].GetComponent<Rigidbody>().isKinematic = false;
                    player[player_number].GetComponent<CapsuleCollider>().enabled = true;
                    player[player_number].GetComponent<Rigidbody>().mass = 1e-07f;
                    GameObject.Find("Sync_Variable_Manager(Clone)").GetComponent<SyncVariableManager>().Timer_Start_Sync();

                }
            }

            else if (game_state == STOPPING_GAME_TIMER)
            {
                measure_time -= Time.deltaTime; //スタートしてからの秒数を格納
                
                if(GameObject.Find("Item(Clone)").GetComponent<ItemCatch>().timer_end == false)
                {
                    GameObject.Find("Item(Clone)").GetComponent<ItemCatch>().enabled = false;
                }
                
                if (measure_time < 0)
                {
                    game_center_text.GetComponent<Text>().text = "";
                    if (num_of_game_count <= 0)
                    {
                        game_state = 3;
                    }
                    else
                    {
                        game_state = 0;
                        GameObject.Find("Player" + NetworkInit.player_num.ToString() + "(Clone)").GetComponent<PlayerMove>().enabled = true;
                        GameObject.Find("Item(Clone)").GetComponent<ItemCatch>().enabled = true;
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
