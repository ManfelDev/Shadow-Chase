using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadoutUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoDisplay;
    [SerializeField] private TextMeshProUGUI maxAmmoDisplay;
    private PlayerManager player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerManager>();
    }

    // Update is called once per frame
    void Update()
    {
        ammoDisplay.text = player.Ammo.ToString();
        
        // If the player has the max ammo, display "MAX"
        if (player.Ammo == player.PistolMaxAmmo)
            maxAmmoDisplay.text = "MAX";
        else
            maxAmmoDisplay.text = "";
    }
}
