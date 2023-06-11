using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    // Set a sprite image for the box
    [SerializeField] private Sprite        brokenBoxSprite;
    [SerializeField] private BoxCollider2D brokenBoxCollider;
    [SerializeField] private AudioClip     breakSound;

    private BoxCollider2D      boxCollider;
    private SpriteRenderer     spriteRenderer;

    private AudioSource audioSource { get => FindObjectOfType<SoundManager>().AudioSource; }

    public int HitPoints { get; set ; } = 2;

    // Start is called before the first frame update
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        brokenBoxCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (HitPoints == 1)
        {
            spriteRenderer.sprite = brokenBoxSprite;
            boxCollider.enabled = false;
            brokenBoxCollider.enabled = true;
        }
        else if (HitPoints == 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {   
        if (collision.gameObject.tag == "Bullet")
        {
            Destroy(collision.gameObject);
            HitPoints--;
            audioSource.PlayOneShot(breakSound, 0.7f);
        }
        else if (collision.gameObject.tag == "ThrowableWeapon")
        {
            Destroy(collision.gameObject);
            HitPoints = 0;
            audioSource.PlayOneShot(breakSound, 0.7f);
        }
    }
}
