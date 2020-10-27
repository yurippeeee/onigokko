using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomNameAutoSet : MonoBehaviour
{
    InputField RoomName;
    void Start()
    {
        RoomName = GameObject.Find("UI").transform.Find("LoginUI").transform.Find("RoomName").GetComponent<InputField>();
    }
    public void OnValueChanged(int value)
    {
        RoomName.text = GetComponent<Dropdown>().options[value].text;
    }
}
