using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    Dropdown room_list_from_dropdown;
    InputField room_name_from_input_field;
    int num_of_rooms = 0;
    Text PlayerName;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();   //マスターサーバへ接続
        room_list_from_dropdown = GameObject.Find("UI").transform.Find("LoginUI").transform.Find("RoomList").GetComponent<Dropdown>();
        room_name_from_input_field = GameObject.Find("UI").transform.Find("LoginUI").transform.Find("RoomName").GetComponent<InputField>();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        PlayerName = GameObject.Find("UI").transform.Find("LoginUI").transform.Find("PlayerName").transform.Find("Text").GetComponent<Text>();
    }


    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    
    // マッチングが成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {

        PhotonNetwork.IsMessageQueueRunning = false;
        SceneManager.LoadScene("Stage_Scene");
    }
    
    public override void OnRoomListUpdate(List<RoomInfo> room_list_from_room_info)
    {
        base.OnRoomListUpdate(room_list_from_room_info);
        
        // ルーム一覧更新
        foreach (var room_info in room_list_from_room_info)
        {
            bool room_name_same_flag = false;

            //ルームが存在する場合
            if (!room_info.RemovedFromList)
            {
                foreach (var Dropdown_option in room_list_from_dropdown.options)
                {
                    if (Dropdown_option.text == room_info.Name)
                    {
                        room_name_same_flag = true;
                        break;
                    }
                }
                // ルーム名が同じでないなら、ドロップダウンにルーム名を追加
                if (!room_name_same_flag)
                {
                    room_list_from_dropdown.options.Add(new Dropdown.OptionData { text = room_info.Name });
                    room_list_from_dropdown.RefreshShownValue();
                    room_name_from_input_field.text = room_list_from_dropdown.options[0].text;
                    num_of_rooms++;
                }
            }

            //ルームが削除されたなら、ドロップダウンにあるルーム名を削除
            else
            {
                room_list_from_dropdown.options.Remove(room_list_from_dropdown.options.Find(o => string.Equals(o.text, room_info.Name)));
                room_list_from_dropdown.RefreshShownValue();
                num_of_rooms--;
                if (num_of_rooms != 0)
                {
                    room_name_from_input_field.text = room_list_from_dropdown.options[0].text;
                }
                else
                {
                    room_name_from_input_field.text = "";
                }
                
            }

            //ルームが満員なら、ドロップダウンのルーム名を削除
            if (room_info.PlayerCount == room_info.MaxPlayers)
            {
                room_list_from_dropdown.options.Remove(room_list_from_dropdown.options.Find(o => string.Equals(o.text, room_info.Name)));
                room_list_from_dropdown.RefreshShownValue();
                num_of_rooms--;
                if (num_of_rooms != 0)
                {
                    room_name_from_input_field.text = room_list_from_dropdown.options[0].text;
                }
                else
                {
                    room_name_from_input_field.text = "";
                }
            }
        }
    }
}
