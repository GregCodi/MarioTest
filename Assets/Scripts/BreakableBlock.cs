using UnityEngine;

public class BreakableBlock : MonoBehaviour
{
    [SerializeField] private GameObject brokenBlockPrefab;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.contacts[0].normal.y > 0.5f)
        {
            Instantiate(brokenBlockPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}