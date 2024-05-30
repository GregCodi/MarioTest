using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 2f;
    private bool facingRight = true;
    private Rigidbody2D rb;
    private float lastDirectionChangeTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Движение влево-вправо
        rb.velocity = new Vector2(speed * (facingRight ? 1 : -1), rb.velocity.y);
        if (Time.time - lastDirectionChangeTime > 10f) 
        {
            // Проверка столкновений и изменение направления
            RaycastHit2D hit = Physics2D.Raycast(transform.position, facingRight ? Vector2.right : Vector2.left, 0.5f);
            if (hit.collider != null && !hit.collider.CompareTag("Ground")) 
            {
                facingRight = !facingRight;
                transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
                lastDirectionChangeTime = Time.time; // Обновляем время последнего изменения
            }
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) return; // Пропускаем землю

        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }
}