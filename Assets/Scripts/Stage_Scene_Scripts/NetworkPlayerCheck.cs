using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class NetworkPlayerCheck : MonoBehaviourPunCallbacks
{
    bool is_first_proc = true;

    void Start()
    {
        //　自分で操作する以外のキャラクターの不要な機能は使えないようにしておく
        GetComponent<PlayerMove>().enabled = false;
        
    }
    void Update()
    {
        if (photonView.IsMine && (GameObject.Find("TimeManage_and_NetworkManage").GetComponent<NetworkInit>().is_num_of_player_max == true) && is_first_proc)
        {
            GetComponent<PlayerMove>().enabled = true;
            is_first_proc = false;
        }
    }
}
