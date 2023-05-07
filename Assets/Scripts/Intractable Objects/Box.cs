using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
    // Set a sprite image for the box
    [SerializeField] private Sprite        brokenBox;
    [SerializeField] private BoxCollider2D brokenBoxCollider;

    private BoxCollider2D  boxCollider;
    private SpriteRenderer spriteRenderer;

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
            spriteRenderer.sprite = brokenBox;
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
        }
    }
}
