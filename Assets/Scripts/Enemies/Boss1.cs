using UnityEngine;

public class Boss1 : MonoBehaviour
{
    private Animator animator;
    private float speedX;
    private float speedY;
    private bool charging;

    private float switchInterval;
    private float switchTimer;

    private int lives;
    private int damage;

    void Start()
    {
        lives = 100;
        damage = 20;

        animator = GetComponent<Animator>();
        EnterChargeState();
        AudioManager.Instance.PlaySound(AudioManager.Instance.bossSpawn);
    }

    void Update()
    {
        float playerPosition = PlayerController.Instance.transform.position.x;

        if (switchTimer > 0){
            switchTimer -= Time.deltaTime;
        } else {
            if (charging && transform.position.x > playerPosition){
                EnterPatrolState();
            } else {
                EnterChargeState();
            }
        }

        if (transform.position.y > 3 || transform.position.y < -3){
            speedY *= -1;
        } else if (transform.position.x < playerPosition){
            EnterChargeState();
        }

        bool boost = PlayerController.Instance.boosting;
        float moveX;
        if (boost && !charging){
            moveX = GameManager.Instance.worldSpeed * Time.deltaTime * -0.5f;
        } else {
            moveX = speedX * Time.deltaTime;
        }
        float moveY = speedY * Time.deltaTime;

        transform.position += new Vector3(moveX, moveY);
        if (transform.position.x < -11){
            Destroy(gameObject);
        }
    }

    void EnterPatrolState(){
        speedX = 0;
        speedY = Random.Range(-2f, 2f);
        switchInterval = Random.Range(5f, 10f);
        switchTimer = switchInterval;
        charging = false;
        animator.SetBool("charging", false);
    }

    void EnterChargeState(){
        if (!charging) AudioManager.Instance.PlaySound(AudioManager.Instance.bossCharge);
        speedX = -10f;
        speedY = 0;
        switchInterval = Random.Range(0.6f, 1.3f);
        switchTimer = switchInterval;
        charging = true;
        animator.SetBool("charging", true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle")){
            Asteroid asteroid = collision.gameObject.GetComponent<Asteroid>();
            if (asteroid) asteroid.TakeDamage(damage);
        } else if (collision.gameObject.CompareTag("Player")){
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player) player.TakeDamage(damage);
        }
    }

    public void TakeDamage(int damage){
        AudioManager.Instance.PlayModifiedSound(AudioManager.Instance.hitArmor);
        lives -= damage;
    }
}
