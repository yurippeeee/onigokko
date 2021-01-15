using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerNameSet2 : MonoBehaviourPunCallbacks
{
    void Start()
    {
        /*
        if (photonView.Owner.NickName == "")
        {
            
            string player_name;
            GameObject.Find("TimeManage_and_NetworkManage").transform.Find("Timer_Text").GetComponent<Timer>().player_name_null_num2++;
            player_name = "NoName" + GameObject.Find("TimeManage_and_NetworkManage").transform.Find("Timer_Text").GetComponent<Timer>().player_name_null_num2.ToString();
            GetComponent<Text>().text = player_name;
            
            //GetComponent<Text>().text = "NoName";
        }
        else
        {*/
            GetComponent<Text>().text = photonView.Owner.NickName;
        //}
    }
}
