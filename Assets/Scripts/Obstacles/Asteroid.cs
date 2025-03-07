using System.Collections;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private FlashWhite flashWhite;

    [SerializeField] private GameObject destroyEffect;

    private int lives;
    private int damage;

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

        lives = 5;
        damage = 1;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player")){
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player) player.TakeDamage(damage);
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
