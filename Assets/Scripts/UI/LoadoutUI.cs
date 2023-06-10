using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadoutUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ammoDisplay;
    [SerializeField] private TextMeshProUGUI maxAmmoDisplay;
    [SerializeField] private Image           currentWeaponImage;
    private PlayerManager player;
    private WeaponsClass  currentWeapon;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerManager>();
        currentWeapon = player.CurrentWeapon;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player has changed weapons
        if (currentWeapon != player.CurrentWeapon)
        {
            currentWeapon = player.CurrentWeapon;

            // Update the weapon image to the current weapon
            currentWeaponImage.sprite = currentWeapon.WeaponSprite;
        }

        ammoDisplay.text = player.Ammo.ToString();
        
        // If the player has the max ammo, display "MAX"
        if (player.Ammo == currentWeapon.MaxAmmo)
            maxAmmoDisplay.text = "MAX";
        else
            maxAmmoDisplay.text = "";
    }
}
