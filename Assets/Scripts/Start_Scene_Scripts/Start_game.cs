using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Start_game : MonoBehaviourPunCallbacks
{
    Text RoomName;
    Text PlayerName;

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
        PhotonNetwork.LocalPlayer.NickName = PlayerName.text;
        PhotonNetwork.JoinOrCreateRoom(RoomName.text, RoomOption, TypedLobby.Default);
    }
}
