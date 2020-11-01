using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Threading.Tasks;

public class NetworkInit : MonoBehaviourPunCallbacks
{
    public bool is_num_of_player_max = false;
    Vector3 player_position;
    string game_info_text;
    int time_control = 0;
    float measure_time = 3;
    GameObject Game_Center_Text;

    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
        switch (PhotonNetwork.CurrentRoom.PlayerCount)
        {
            case 1:
                player_position = new Vector3(-11, 0, -3.86f);
                break;
            case 2:
                player_position = new Vector3(11.3f, 0, -8.03f);
                break;
            case 3:
                player_position = new Vector3(11.3f, 0, -3.86f);
                break;
            case 4:
                player_position = new Vector3(7.2f, 0, -3.86f);
                break;
            default: break;
        }
        PhotonNetwork.Instantiate("Player" + PhotonNetwork.CurrentRoom.PlayerCount.ToString(), player_position, Quaternion.identity, 0).name = "Player" + PhotonNetwork.CurrentRoom.PlayerCount.ToString() + "(Clone)";
        GameObject.Find("Player" + PhotonNetwork.CurrentRoom.PlayerCount.ToString() + "(Clone)").transform.Find("Player_Camera").GetComponent<Camera>().enabled = true;
        game_info_text = "マッチメイク中…";
        Game_Center_Text = GameObject.Find("Game_Center_Text");
        Game_Center_Text.GetComponent<Text>().text = game_info_text;
    }
    void Update()
    {
        switch (time_control)
        {
            case 0:
                if (GameObject.Find("Player4(Clone)"))
                {
                    PhotonNetwork.Instantiate("Sync_Variable_Manager", new Vector3(0, 0, 0), Quaternion.identity, 0).name = "Sync_Variable_Manager(Clone)";
                    game_info_text = "マッチメイク完了!";
                    Game_Center_Text.GetComponent<Text>().text = game_info_text;
                    time_control = 1;
                }
                break;

            case 1:
                measure_time -= Time.deltaTime; //スタートしてからの秒数を格納
                if (measure_time < 0)
                {
                    game_info_text = "ゲームスタート!";
                    Game_Center_Text.GetComponent<Text>().text = game_info_text;
                    measure_time = 3;
                    time_control = 2;
                }
                break;

            case 2:
                measure_time -= Time.deltaTime; //スタートしてからの秒数を格納
                if (measure_time < 0)
                {
                    game_info_text = "";
                    Game_Center_Text.GetComponent<Text>().text = game_info_text;
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
}
