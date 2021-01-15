using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Start_game : MonoBehaviourPunCallbacks
{
    Text RoomName;
    Text PlayerName;
    public AudioClip click_sound;
    public static string player_name;

    void Start()
    {
        RoomName = GameObject.Find("UI").transform.Find("LoginUI").transform.Find("RoomName").transform.Find("Text").GetComponent<Text>();
        PlayerName = GameObject.Find("UI").transform.Find("LoginUI").transform.Find("PlayerName").transform.Find("Text").GetComponent<Text>();
    }

    static RoomOptions RoomOption = new RoomOptions()
    {
        MaxPlayers = 4, //入室可能な最大人数
        IsOpen = true, //部屋に参加できるか
        IsVisible = true, //この部屋がロビーにリストされるか
    };

    
    public void JoinRoom_and_ChangeScene()
    {
        RoomOption.CustomRoomProperties = new Hashtable()
        { { "Player1", false },
          { "Player2", false },
          { "Player3", false },
          { "Player4", false },
          { "is_entering", 1 }};

        player_name = PlayerName.text;
        AudioSource.PlayClipAtPoint(click_sound, transform.position);
        PhotonNetwork.JoinOrCreateRoom(RoomName.text, RoomOption, TypedLobby.Default);
        
        
    }
}
