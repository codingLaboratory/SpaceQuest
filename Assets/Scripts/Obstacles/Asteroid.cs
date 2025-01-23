using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    [SerializeField] private Sprite[] sprites;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        float pushX = Random.Range(-1f,0);
        float pushY = Random.Range(-1f,1f);
        rb.linearVelocity = new Vector2(pushX,pushY);
    }
    
    void Update()
    {
        float moveX = (GameManager.Instance.worldSpeed * PlayerController.Instance.boost) * Time.deltaTime;
        transform.position += new Vector3(-moveX, 0);
        if (transform.position.x < -11){
            Destroy(gameObject);
        }
    }
}
