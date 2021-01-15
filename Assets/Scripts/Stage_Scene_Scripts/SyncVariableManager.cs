using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Data;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class SyncVariableManager : MonoBehaviourPunCallbacks
{
    public int player_exist_num = 13; //プレイヤー全員が減算命令を実行するため、4人×3人死亡+1人(勝者)=13
    float damage = 0.20f;
    public bool timer_start_sync = false;
    public AudioClip enemy_hit_sound;
    public AudioClip attack_sound;
    DataTable player_rank_table;
    GameObject timer_text;
    public bool is_item_grab = false;
    Room room;
    Hashtable cp;


    void Start()
    {
        player_rank_table = GameObject.Find("TimeManage_and_NetworkManage").transform.Find("Timer_Text").GetComponent<Timer>().player_rank_table;
        timer_text = GameObject.Find("TimeManage_and_NetworkManage").transform.Find("Timer_Text").gameObject;
        room = PhotonNetwork.CurrentRoom;
        cp = room.CustomProperties;
    }

    void Player_Dead()
    {
        photonView.RPC(nameof(NumOfPlayerDecrement), RpcTarget.All, new object[] { });
    }

    public void Damage(string PlayerObjectName)
    {
        photonView.RPC(nameof(DamageMethod), RpcTarget.All, new object[] { PlayerObjectName });
    }

    public void Timer_Start_Sync()
    {
        photonView.RPC(nameof(TimerStartSync), RpcTarget.All, new object[] { });
    }

    public void Play_Attack_Sound()
    {
        photonView.RPC(nameof(PlayAttackSound), RpcTarget.All, new object[] { });
    }

    public void Set_Game_Rule(int game_count, float time)
    {
        photonView.RPC(nameof(SetGameRule), RpcTarget.All, new object[] { game_count, time});
    }

    public void Grabbed_Item()
    {
        photonView.RPC(nameof(GrabbedItem), RpcTarget.All, new object[] { });
    }

    public void Threw_Item()
    {
        photonView.RPC(nameof(ThrewItem), RpcTarget.All, new object[] { });
    }
    
    /*
    public void Set_Custom_Property()
    {
        photonView.RPC(nameof(SetCustomProperty), RpcTarget.MasterClient, new object[] { });
    }
    */

    [PunRPC]
    void NumOfPlayerDecrement(PhotonMessageInfo info)
    {
        --player_exist_num;
    }

    /*
    [PunRPC]
    void SetCustomProperty(PhotonMessageInfo info)
    {
        PhotonNetwork.CurrentRoom.SetCustomProperties(cp);
    }
    */

    [PunRPC]
    void GrabbedItem(PhotonMessageInfo info)
    {
        is_item_grab = true;
    }

    [PunRPC]
    void ThrewItem(PhotonMessageInfo info)
    {
        is_item_grab = false;
    }

    [PunRPC]
    void DamageMethod(string PlayerObjectName, PhotonMessageInfo info)
    {
        GameObject obj = (GameObject)Resources.Load("HitEffect");
        Vector3 pos = GameObject.Find(PlayerObjectName).transform.position;
        pos.y += 0.5f;
        Instantiate(obj, pos, Quaternion.identity);

        GameObject.Find(PlayerObjectName).transform.Find("HPUI").transform.Find("HPBar").GetComponent<Slider>().value -= damage;
        string player_num = GameObject.Find(PlayerObjectName).name.Substring(6,1);

        GameObject.Find("PlayerName"+player_num+"(Clone)").transform.Find("HPBar").GetComponent<Slider>().value -= damage;

        if (GameObject.Find(PlayerObjectName).transform.Find("HPUI").transform.Find("HPBar").GetComponent<Slider>().value <= 0.41f)
        {
            GameObject.Find(PlayerObjectName).transform.Find("HPUI").transform.Find("HPBar").transform.Find("Fill Area").transform.Find("Fill").GetComponent<Image>().color = new Color(1.0f, 0, 0, 1.0f);
            GameObject.Find("PlayerName" + player_num + "(Clone)").transform.Find("HPBar").transform.Find("Fill Area").transform.Find("Fill").GetComponent<Image>().color = new Color(1.0f, 0, 0, 1.0f);
        }
            
        string player_name = GameObject.Find(PlayerObjectName).transform.Find("HPUI").transform.Find("PlayerName").GetComponent<Text>().text;
        DataRow[] dRows = player_rank_table.Select("Player_Name = '"+ player_name+"'");
        foreach (var row in dRows)
        {
            player_rank_table.Rows[player_rank_table.Rows.IndexOf(row)]["HP"] = (int)player_rank_table.Rows[player_rank_table.Rows.IndexOf(row)]["HP"]  - (int)(damage * 100); 
        }
        
        AudioSource.PlayClipAtPoint(enemy_hit_sound, transform.position);

        if (GameObject.Find(PlayerObjectName).transform.Find("HPUI").transform.Find("HPBar").GetComponent<Slider>().value <= 0.19f)
        {
            //photonView.RequestOwnership();
            Destroy(GameObject.Find(PlayerObjectName).gameObject);
            Player_Dead();
        }
    }

    [PunRPC]
    void TimerStartSync(PhotonMessageInfo info)
    {
        timer_start_sync = true;
    }

    [PunRPC]
    void PlayAttackSound(PhotonMessageInfo info)
    {
        AudioSource.PlayClipAtPoint(attack_sound, transform.position);
    }

    [PunRPC]
    void SetGameRule(int game_count, float time, PhotonMessageInfo info)
    {
        timer_text.GetComponent<Timer>().num_of_game_count = game_count;
        timer_text.GetComponent<Timer>().game_time = time;
        GameObject.Find("TimeManage_and_NetworkManage").GetComponent<NetworkInit>().is_setting_completed = true;
    }
}
