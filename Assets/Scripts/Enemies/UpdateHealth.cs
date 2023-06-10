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

    // Start is called before the first frame update
    void Start()
    {
        enemyManager = GetComponentInParent<EnemyManager>();
        enemyMovement = GetComponentInParent<EnemyMovement>();
        followPlayer = GameObject.FindObjectOfType<FollowPlayer>();

        localScale = transform.localScale;
        maxScale = localScale.x;
        enemyTransform = GetHighestParentTransform(transform);
    }

    // Update is called once per frame
    void Update()
    {
        localScale.x = (float) maxScale * enemyManager.GetCurrentHealth() / enemyManager.GetMaxHealth();
        localRotation = enemyTransform.localRotation;

        transform.localScale = localScale;
        transform.parent.transform.localRotation = localRotation;
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
