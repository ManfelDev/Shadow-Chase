using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAlarm : MonoBehaviour
{
    [SerializeField] private bool        alarm = false;
    [SerializeField] private CanvasGroup alarmLight;

    public bool               IsON { get => alarm; }
    private float             slider;
    private bool              sliderUp;
    private bool              sliderDown;
    private EnemyRaycast[]    enemyRaycasts;
    private RunningManRaycast runningManRaycast;

    void Start()
    {
        slider = 0.0f;
        sliderDown = false;
        sliderUp = true;
        enemyRaycasts = FindObjectsOfType<EnemyRaycast>();
        runningManRaycast = FindObjectOfType<RunningManRaycast>();
    }

    void Update()
    {
        if (IsON)
        {
            if(slider > 1)
            {
                slider = 1.0f;
                sliderDown = true;
                sliderUp = false;
            }
            else if (slider < 0)
            {
                slider = 0.0f;
                sliderDown = false;
                sliderUp = true;
            }

            if (sliderUp)
                slider += 0.008f;
            
            else if (sliderDown)
                slider -= 0.008f;


            alarmLight.alpha= slider;
        }

        else alarmLight.alpha= 0;
    }


    public void Trigger()
    {
        alarm = true;
    }


    public void SlowTriggerAll()
    {
        runningManRaycast.SlowTrigger();
        foreach (EnemyRaycast r in enemyRaycasts)
        {
            r.SlowTrigger();
        }
    }
}