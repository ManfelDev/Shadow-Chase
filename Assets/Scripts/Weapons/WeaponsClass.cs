using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsClass
{
    public float  FireRate { get; private set; }
    public float  Damage { get; private set; }
    public int    MaxAmmo { get; private set; }
    public string Tag { get; private set; }

    public WeaponsClass(float fireRate, int damage, int maxAmmo, string tag)
    {
        FireRate = fireRate;
        Damage = damage;
        MaxAmmo = maxAmmo;
        Tag = tag;
    }

    // Weapon (Pistol) with a get
    public static WeaponsClass Pistol
    {
        get
        {
            return new WeaponsClass(0.25f, 25, 12, "Pistol");
        }
    }

    // Weapon (AK) with a get
    public static WeaponsClass AK
    {
        get
        {
            return new WeaponsClass(0.1f, 10, 40, "AK");
        }
    }
}