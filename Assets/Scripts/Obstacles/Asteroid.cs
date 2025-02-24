using System.Collections;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private Material defaultMaterial;
    [SerializeField] private Material whiteMaterial;

    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private int lives;

    [SerializeField] private Sprite[] sprites;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        float pushX = Random.Range(-1f,0);
        float pushY = Random.Range(-1f,1f);
        rb.linearVelocity = new Vector2(pushX,pushY);
        float randomScale = Random.Range(0.6f, 1f);
        transform.localScale = new Vector2(randomScale, randomScale);
    }
    
    void Update()
    {
        float moveX = (GameManager.Instance.worldSpeed * PlayerController.Instance.boost) * Time.deltaTime;
        transform.position += new Vector3(-moveX, 0);
        if (transform.position.x < -11){
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Bullet")){
            spriteRenderer.material = whiteMaterial;
            StartCoroutine("ResetMaterial");
            AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.hitRock);
            lives--;
            if (lives <= 0){
                Instantiate(destroyEffect, transform.position, transform.rotation);
                AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.boom2);
                Destroy(gameObject);
            }
        }
    }

    IEnumerator ResetMaterial(){
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.material = defaultMaterial;
    }
}
