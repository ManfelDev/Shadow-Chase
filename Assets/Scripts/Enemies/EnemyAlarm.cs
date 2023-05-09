using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlarm : MonoBehaviour
{
    [SerializeField] private bool alarm = false;
    public bool IsON { get => alarm; }

    public void Trigger()
    {
        alarm = true;
    }
}