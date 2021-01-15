using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class LeaveRoom : MonoBehaviourPunCallbacks
{
    public void OnClicked()
    {
        GameObject.Find("TimeManage_and_NetworkManage").GetComponent<NetworkInit>().LeftRoom();
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Start_Scene");
    }
}
