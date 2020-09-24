using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHit : MonoBehaviour
{
    float damage = 0.34f;
    public bool attack_collision_flag = false;
    
    public GameObject hpbar;
    
    //オブジェクトと接触した瞬間に呼び出される
    void OnCollisionEnter(Collision other)
    {
        //攻撃した相手がEnemyの場合
        if (other.gameObject.CompareTag("Enemy"))
        {
            if(attack_collision_flag == false)
            {
                hpbar.GetComponent<Slider>().value -= damage;
                if (hpbar.GetComponent<Slider>().value <= 0)
                {
                    Destroy(other.gameObject);
                }
                attack_collision_flag = true;
            }
            
            
        }
        
    }
}
