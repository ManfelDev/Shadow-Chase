using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateHealth : MonoBehaviour
{
    private EnemyManager enemyManager;
    private EnemyMovement enemyMovement;
    private float maxScale;
    private Vector3 localScale;
    private Vector3 localPosition;

    // Start is called before the first frame update
    void Start()
    {
        enemyManager = GetComponentInParent<EnemyManager>();
        enemyMovement = GetComponentInParent<EnemyMovement>();

        localScale = transform.localScale;
        maxScale = localScale.x;
        localPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        localScale.x = (float) maxScale * enemyManager.GetCurrentHealth() / enemyManager.GetMaxHealth();
        localPosition.x = (float) -enemyMovement.GetEnemySpeedX() * (maxScale - localScale.x) / 2;

        transform.localScale = localScale;
        transform.localPosition = localPosition;
    }
}
