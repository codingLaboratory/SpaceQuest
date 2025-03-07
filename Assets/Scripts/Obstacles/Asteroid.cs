using System.Collections;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private FlashWhite flashWhite;

    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private int lives;

    [SerializeField] private Sprite[] sprites;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        flashWhite = GetComponent<FlashWhite>();

        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        float pushX = Random.Range(-1f,0);
        float pushY = Random.Range(-1f,1f);
        rb.linearVelocity = new Vector2(pushX,pushY);
        float randomScale = Random.Range(0.6f, 1f);
        transform.localScale = new Vector2(randomScale, randomScale);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Bullet")){
            TakeDamage(1);
        } else if (collision.gameObject.CompareTag("Boss")){
            TakeDamage(10);
        }
    }

    public void TakeDamage(int damage){
        AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.hitRock);
        lives -= damage;
        flashWhite.Flash();
        if (lives <= 0){
            Instantiate(destroyEffect, transform.position, transform.rotation);
            AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.boom2);
            Destroy(gameObject);
        }
    }
}
