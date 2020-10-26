using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    Dropdown ROOM_LIST_from_Dropdown;
    InputField ROOM_NAME_from_Input_Field;
    int num_of_rooms = 0;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();   //マスターサーバへ接続
        ROOM_LIST_from_Dropdown = GameObject.Find("UI").transform.Find("LoginUI").transform.Find("RoomList").GetComponent<Dropdown>();
        ROOM_NAME_from_Input_Field = GameObject.Find("UI").transform.Find("LoginUI").transform.Find("RoomName").GetComponent<InputField>();
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
    
    public override void OnRoomListUpdate(List<RoomInfo> ROOM_LIST_from_RoomInfo)
    {
        base.OnRoomListUpdate(ROOM_LIST_from_RoomInfo);
        
        // ルーム一覧更新
        foreach (var RoomInfo in ROOM_LIST_from_RoomInfo)
        {
            bool room_name_same_flag = false;

            //ルームが存在する場合
            if (!RoomInfo.RemovedFromList)
            {
                foreach (var Dropdown_option in ROOM_LIST_from_Dropdown.options)
                {
                    if (Dropdown_option.text == RoomInfo.Name)
                    {
                        room_name_same_flag = true;
                        break;
                    }
                }
                // ルーム名が同じでないなら、ドロップダウンにルーム名を追加
                if (!room_name_same_flag)
                {
                    ROOM_LIST_from_Dropdown.options.Add(new Dropdown.OptionData { text = RoomInfo.Name });
                    ROOM_LIST_from_Dropdown.RefreshShownValue();
                    ROOM_NAME_from_Input_Field.text = ROOM_LIST_from_Dropdown.options[0].text;
                    num_of_rooms++;
                }
            }

            //ルームが削除されたなら、ドロップダウンにあるルーム名を削除
            else
            {
                ROOM_LIST_from_Dropdown.options.Remove(ROOM_LIST_from_Dropdown.options.Find(o => string.Equals(o.text, RoomInfo.Name)));
                ROOM_LIST_from_Dropdown.RefreshShownValue();
                num_of_rooms--;
                if (num_of_rooms != 0)
                {
                    ROOM_NAME_from_Input_Field.text = ROOM_LIST_from_Dropdown.options[0].text;
                }
                else
                {
                    ROOM_NAME_from_Input_Field.text = "";
                }
                
            }

            //ルームが満員なら、ドロップダウンのルーム名を削除
            if (RoomInfo.PlayerCount == RoomInfo.MaxPlayers)
            {
                ROOM_LIST_from_Dropdown.options.Remove(ROOM_LIST_from_Dropdown.options.Find(o => string.Equals(o.text, RoomInfo.Name)));
                ROOM_LIST_from_Dropdown.RefreshShownValue();
                num_of_rooms--;
                if (num_of_rooms != 0)
                {
                    ROOM_NAME_from_Input_Field.text = ROOM_LIST_from_Dropdown.options[0].text;
                }
                else
                {
                    ROOM_NAME_from_Input_Field.text = "";
                }
            }
        }
    }
}
