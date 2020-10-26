using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerNameSet : MonoBehaviourPunCallbacks
{
    void Start()
    {
            GetComponent<Text>().text = photonView.Owner.NickName;
    }
}
