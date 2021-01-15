using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkPlayerCheck : MonoBehaviourPunCallbacks
{
    bool is_first_proc = true;
    public bool is_mine = false;
    void Start()
    {
        GetComponent<PlayerMove>().enabled = false;
        GetComponent<Animator>().speed = 2.0f;
    }
    void Update()
    {
        if (photonView.IsMine && (GameObject.Find("TimeManage_and_NetworkManage").GetComponent<NetworkInit>().is_num_of_player_max == true) && is_first_proc)
        {
            is_mine = true;
            GetComponent<PlayerMove>().enabled = true;
            is_first_proc = false;
        }
    }
}
