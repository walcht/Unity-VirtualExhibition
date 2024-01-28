using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InstantiateText : MonoBehaviour
{
    public Transform toilet_door_01;
    public Transform toilet_door_02;
    public Transform toilet_door_03;
    public Transform toilet_door_04;
    public TextMeshPro toilet_text;

    public Transform main_entrance_door;
    public TextMeshPro main_entrance_text;

    public Transform left_exit_door;
    public TextMeshPro left_exit_text;

    public Transform right_exit_door;
    public TextMeshPro right_exit_text; 

    void Awake()
    {
        Instantiate(toilet_text, toilet_door_01);
        Instantiate(toilet_text, toilet_door_02);
        Instantiate(toilet_text, toilet_door_03);
        Instantiate(toilet_text, toilet_door_04);

        Instantiate(main_entrance_text, main_entrance_door);

        Instantiate(left_exit_text, left_exit_door);
        Instantiate(right_exit_text, right_exit_door);
    }
}
