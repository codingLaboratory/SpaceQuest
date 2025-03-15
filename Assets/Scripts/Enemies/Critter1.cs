using UnityEngine;

public class Critter1 : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;

    private float moveSpeed;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private float moveTimer;
    private float moveInterval;
    [SerializeField] private GameObject zappedEffect;
    [SerializeField] private GameObject burnEffect;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
        moveSpeed = Random.Range(0.5f, 3f);
        GenerateRandomPosition();
        moveInterval = Random.Range(0.1f, 2f);
        moveTimer = moveInterval;
    }

    void Update()
    {
        if (moveTimer > 0){
            moveTimer -= Time.deltaTime;
        } else {
            GenerateRandomPosition();
            moveInterval = Random.Range(0.3f, 2f);
            moveTimer = moveInterval;
        }

        targetPosition -= new Vector3(GameManager.Instance.worldSpeed * Time.deltaTime, 0);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        Vector3 relativePos = targetPosition - transform.position;
        if (relativePos != Vector3.zero){
            targetRotation = Quaternion.LookRotation(Vector3.forward, relativePos);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 1080 * Time.deltaTime);
        }
    }

    private void GenerateRandomPosition(){
        float randomX = Random.Range(-5f, 5f);
        float randomY = Random.Range(-5f, 5f);
        targetPosition = new Vector2(randomX, randomY);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet")){
            Instantiate(zappedEffect, transform.position, transform.rotation);
            Destroy(gameObject);
            AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.squished);
            GameManager.Instance.critterCounter++;
        } else if (collision.gameObject.CompareTag("Player")){
            Instantiate(burnEffect, transform.position, transform.rotation);
            Destroy(gameObject);
            AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.burn);
            GameManager.Instance.critterCounter++;
        }
    }
}
