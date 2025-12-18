using UnityEngine;

public class ObstacleScroller : MonoBehaviour
{
    void FixedUpdate()
    {
        float velocity = InfiniteMapManager.CurrentMoveVelocity;

        transform.Translate(velocity * Time.fixedDeltaTime, 0, 0);

        if (transform.position.x < -20f) 
        {
            Destroy(gameObject);
        }
        else if (transform.position.x > 30f + 10f) 
        {
            Destroy(gameObject);
        }
    }
}