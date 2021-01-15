using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkInit : MonoBehaviourPunCallbacks
{

    public bool is_num_of_player_max = false;
    Vector3 player_position;
    Quaternion player_rotation;
    Vector3 player_name_object_position;
    string game_info_text;
    int time_control = 0;
    float measure_time = 3;
    public bool is_setting_completed = false;
    int game_count;
    float time;
    public static int player_num;
    bool is_first_process = true;
    int my_number;

    GameObject game_center_text;
    GameObject num_of_change;
    GameObject num_of_second;
    GameObject text_num_of_change;
    GameObject text_num_of_second;
    GameObject decision_button;
    GameObject leave_room_button;
    Room room;
    Hashtable cp;



    void Start()
    {
        my_number = PhotonNetwork.CurrentRoom.PlayerCount;
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            PhotonNetwork.Instantiate("Sync_Variable_Manager", new Vector3(0, 0, 0), Quaternion.identity, 0).name = "Sync_Variable_Manager(Clone)";
        }
        num_of_change = GameObject.Find("TimeManage_and_NetworkManage").transform.Find("NumOfChange").gameObject;
        num_of_second = GameObject.Find("TimeManage_and_NetworkManage").transform.Find("NumOfSecond").gameObject;
        text_num_of_change = GameObject.Find("TimeManage_and_NetworkManage").transform.Find("Text_NumOfChange").gameObject;
        text_num_of_second = GameObject.Find("TimeManage_and_NetworkManage").transform.Find("Text_NumOfSecond").gameObject;
        decision_button = GameObject.Find("TimeManage_and_NetworkManage").transform.Find("DecisionButton").gameObject;
        leave_room_button = GameObject.Find("TimeManage_and_NetworkManage").transform.Find("LeaveRoomButton").gameObject;

        room = PhotonNetwork.CurrentRoom;
        cp = room.CustomProperties;

        num_of_change.SetActive(false);
        num_of_second.SetActive(false);
        text_num_of_change.SetActive(false);
        text_num_of_second.SetActive(false);
        decision_button.SetActive(false);

        Debug.developerConsoleVisible = false;
        PhotonNetwork.IsMessageQueueRunning = true;
    }
    void Update()
    {
        int is_entering = (int)cp["is_entering"];
        
        if (is_first_process && (is_entering == my_number))
        {

            for (int i = 1; i <= 4; i++)
            {
                bool is_player = (bool)cp["Player" + i.ToString()];
                if (is_player == false)
                {
                    cp["Player" + i.ToString()] = true;
                    player_num = i;
                    PhotonNetwork.CurrentRoom.SetCustomProperties(cp);
                    if (Start_game.player_name == "")
                    {
                        PhotonNetwork.LocalPlayer.NickName = "NoName " + player_num.ToString();
                    }
                    else
                    {
                        PhotonNetwork.LocalPlayer.NickName = Start_game.player_name;
                    }
                    break;
                }
            }

            

            switch (player_num)
            {
                case 1:
                    player_position = new Vector3(8.8f, -6.23f, 13.17f);
                    player_rotation = Quaternion.Euler(0, -50f, 0);
                    player_name_object_position = new Vector3(300.0f, 110.0f, 0.0f);
                    PhotonNetwork.Instantiate("Item", new Vector3(6.19f, -6.87f, 16.82f), Quaternion.identity, 0).name = "Item(Clone)";
                    //PhotonNetwork.Instantiate("Item", new Vector3(0.55f, -6.87f, 12.19f), Quaternion.identity, 0).name = "Item(Clone)";
                    
                    break;
                case 2:
                    player_position = new Vector3(3.69f, -6.23f, 21.06f);
                    player_rotation = Quaternion.Euler(0, 160f, 0);
                    player_name_object_position = new Vector3(300.0f, 30.0f, 0.0f);
                    break;
                case 3:
                    player_position = new Vector3(-3.26f, -6.23f, 19.55f);
                    player_rotation = Quaternion.Euler(0, 110f, 0);
                    player_name_object_position = new Vector3(300.0f, -50.0f, 0.0f);

                    break;
                case 4:
                    player_position = new Vector3(2.9f, -6.23f, 10.6f);
                    player_rotation = Quaternion.Euler(0, 10f, 0);
                    player_name_object_position = new Vector3(300.0f, -130.0f, 0.0f);
                    break;
                default: break;
            }
            PhotonNetwork.Instantiate("Player" + player_num.ToString(), player_position, player_rotation, 0).name = "Player" + player_num.ToString() + "(Clone)";

            GameObject.Find("Player" + player_num.ToString() + "(Clone)").transform.Find("Player_Camera").GetComponent<Camera>().enabled = true;

            PhotonNetwork.Instantiate("PlayerName" + player_num.ToString(), player_name_object_position, Quaternion.identity, 0).name = "PlayerName" + player_num.ToString() + "(Clone)";

            game_info_text = "マッチメイク中…";
            game_center_text = GameObject.Find("Game_Center_Text");
            game_center_text.GetComponent<Text>().text = game_info_text;

            if (PhotonNetwork.IsMasterClient)
            {
                num_of_change.SetActive(true);
                num_of_second.SetActive(true);
                text_num_of_change.SetActive(true);
                text_num_of_second.SetActive(true);
                decision_button.SetActive(true);
            }

            cp["is_entering"] = (int)cp["is_entering"] + 1;
            PhotonNetwork.CurrentRoom.SetCustomProperties(cp);

            is_first_process = false;
        }


        switch (time_control)
        {
            case 0:
                if (GameObject.Find("Player1(Clone)") && GameObject.Find("Player2(Clone)") && GameObject.Find("Player3(Clone)") && GameObject.Find("Player4(Clone)") && is_setting_completed)
                {
                    leave_room_button.SetActive(false);
                    for (int i=1; i<=4; i++)
                    {
                        GameObject.Find("PlayerName" + i.ToString() + "(Clone)").transform.SetParent(GameObject.Find("TimeManage_and_NetworkManage").transform, false);
                    }
                    if (PhotonNetwork.IsMasterClient)
                    {
                        
                        GameObject.Find("Sync_Variable_Manager(Clone)").GetComponent<SyncVariableManager>().Set_Game_Rule(game_count, time);
                    }
                    game_info_text = "マッチメイク完了!";
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    game_center_text.GetComponent<Text>().text = game_info_text;
                    time_control = 1;
                }
                break;

            case 1:
                measure_time -= Time.deltaTime; //スタートしてからの秒数を格納
                if (measure_time < 0)
                {
                    game_info_text = "ゲームスタート!";
                    game_center_text.GetComponent<Text>().text = game_info_text;
                    measure_time = 3;
                    time_control = 2;
                }
                break;

            case 2:
                measure_time -= Time.deltaTime; //スタートしてからの秒数を格納
                if (measure_time < 0)
                {
                    game_info_text = "";
                    game_center_text.GetComponent<Text>().text = game_info_text;
                    is_num_of_player_max = true;
                    time_control = 3;
                }
                break;
            case 3:
                break;
            default:
                break;
        }
        
    }

    public void Setting_Change()
    {
        game_count = int.Parse(num_of_change.GetComponent<Dropdown>().options[num_of_change.GetComponent<Dropdown>().value].text);
        time = float.Parse(num_of_second.GetComponent<Dropdown>().options[num_of_second.GetComponent<Dropdown>().value].text);
        is_setting_completed = true;

        num_of_change.SetActive(false);
        num_of_second.SetActive(false);
        text_num_of_change.SetActive(false);
        text_num_of_second.SetActive(false);
        decision_button.SetActive(false);
    }

    override public void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Start_Scene");
    }

    public void LeftRoom()
    {
        cp["is_entering"] = (int)cp["is_entering"] - 1;
        PhotonNetwork.CurrentRoom.SetCustomProperties(cp);
        is_first_process = true;
        cp["Player" + player_num.ToString()] = false;
        PhotonNetwork.CurrentRoom.SetCustomProperties(cp);
    }

}
