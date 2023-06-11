using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsClass
{
    public float     FireRate { get; private set; }
    public float     Damage { get; private set; }
    public int       MaxAmmo { get; private set; }
    public string    Tag { get; private set; }
    public bool      IsSilenced { get; private set; }
    public Sprite    WeaponSprite { get; private set; }
    public Sprite    RightArmSprite { get; private set; }
    public Sprite    LeftArmSprite { get; private set; }
    public AudioClip ShootSound { get; private set; }
    public AudioClip BlankShootSound { get; private set; }

    public WeaponsClass(float fireRate, int damage, int maxAmmo, string tag, bool isSilenced, 
                        Sprite weaponSprite, Sprite rightArmSprite, Sprite leftArmSprite, 
                        AudioClip shootSound, AudioClip blankShootSound)
    {
        FireRate = fireRate;
        Damage = damage;
        MaxAmmo = maxAmmo;
        Tag = tag;
        IsSilenced = isSilenced;
        WeaponSprite = weaponSprite;
        RightArmSprite = rightArmSprite;
        LeftArmSprite = leftArmSprite;
        ShootSound = shootSound;
        BlankShootSound = blankShootSound;
    }

    // Weapon (Pistol) with a get
    public static WeaponsClass Pistol
    {
        get
        {
            return new WeaponsClass(0.5f, 25, 12, "Pistol", true,
                                    Resources.Load<Sprite>("Weapons/pistol"),
                                    Resources.Load<Sprite>("Arms/Pistol arms/right_arm_pistol"),
                                    Resources.Load<Sprite>("Arms/Pistol arms/left_arm_pistol"),
                                    Resources.Load<AudioClip>("Audio/silencedshot"),
                                    Resources.Load<AudioClip>("Audio/pistol empty"));
        }
    }

    // Weapon (AK) with a get
    public static WeaponsClass AK
    {
        get
        {
            return new WeaponsClass(0.2f, 15, 30, "AK", false,
                                    Resources.Load<Sprite>("Weapons/ak"),
                                    Resources.Load<Sprite>("Arms/AK arms/right_arm_ak"),
                                    Resources.Load<Sprite>("Arms/AK arms/left_arm_ak"),
                                    Resources.Load<AudioClip>("Audio/akshot"),
                                    Resources.Load<AudioClip>("Audio/AK empty"));
        }
    }

    // Punch with a get
    public static WeaponsClass Punch
    {
        get
        {
            return new WeaponsClass(0.5f, 30, 0, null, true,
                                    null,
                                    null,
                                    null,
                                    null,
                                    null);
        }
    }
}