using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateHealth : MonoBehaviour
{
    private EnemyManager  enemyManager;
    private EnemyMovement enemyMovement;
    private FollowPlayer  followPlayer;
    private float         maxScale;
    private Vector3       localScale;
    private Quaternion    localRotation;
    private Transform     enemyTransform;
    private EnemyRaycast  enemyRaycast;

    // Start is called before the first frame update
    void Start()
    {
        enemyManager = GetComponentInParent<EnemyManager>();
        enemyMovement = GetComponentInParent<EnemyMovement>();
        followPlayer = GameObject.FindObjectOfType<FollowPlayer>();
        enemyRaycast = GetComponentInParent<EnemyRaycast>();

        localScale = transform.localScale;
        maxScale = localScale.x;
        enemyTransform = GetHighestParentTransform(transform);
    }

    // Update is called once per frame
    void Update()
    {
        // Sets the health bar scale and rotation to match the current health and direction
        localScale.x = (float) maxScale * enemyManager.GetCurrentHealth() / enemyManager.GetMaxHealth();
        localRotation = enemyTransform.localRotation;

        transform.localScale = localScale;
        transform.parent.transform.localRotation = localRotation;

        if (enemyManager.GetCurrentHealth() < enemyManager.GetMaxHealth())
            enemyRaycast.SlowTrigger();
    }

    private Transform GetHighestParentTransform(Transform transform)
    {
        Transform highestParent = transform;
        while (highestParent.parent != null)
        {
            highestParent = highestParent.parent;
        }
        return highestParent;
    }
}
