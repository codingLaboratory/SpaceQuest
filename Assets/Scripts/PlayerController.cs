using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Material defaultMaterial;
    [SerializeField] private Material whiteMaterial;

    private Vector2 playerDirection;
    [SerializeField] private float moveSpeed;
    public float boost = 1f;
    private float boostPower = 4f;
    private bool boosting = false;

    [SerializeField] private float energy;
    [SerializeField] private float maxEnergy;
    [SerializeField] private float energyRegen;

    [SerializeField] private float health;
    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject destroyEffect;
    [SerializeField] private ParticleSystem engineEffect;

    void Awake(){
        if (Instance != null){
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMaterial = spriteRenderer.material;

        energy = maxEnergy;
        UIController.Instance.UpdateEnergySlider(energy, maxEnergy);
        health = maxHealth;
        UIController.Instance.UpdateHealthSlider(health, maxHealth);
    }

    void Update()
    {
        if (Time.timeScale > 0){
            float directionX = Input.GetAxisRaw("Horizontal");
            float directionY = Input.GetAxisRaw("Vertical");

            animator.SetFloat("moveX", directionX);
            animator.SetFloat("moveY", directionY);
            
            playerDirection = new Vector2(directionX, directionY).normalized;

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Fire2")){
                EnterBoost();
            } else if (Input.GetKeyUp(KeyCode.Space) || Input.GetButtonUp("Fire2")){
                ExitBoost();
            }

            if (Input.GetKeyDown(KeyCode.RightShift) || Input.GetButtonDown("Fire1")){
                PhaserWeapon.Instance.Shoot();
            }
        }
    }

    void FixedUpdate(){
        rb.linearVelocity = new Vector2(playerDirection.x * moveSpeed, playerDirection.y * moveSpeed);

        if (boosting){
            if (energy >= 0.2f) energy -= 0.2f;
            else {
                ExitBoost();
            }
        } else {
            if (energy < maxEnergy){
                energy += energyRegen;
            }
        }
        UIController.Instance.UpdateEnergySlider(energy, maxEnergy);
    }

    private void EnterBoost(){
        if (energy > 10){
            AudioManager.Instance.PlaySound(AudioManager.Instance.fire);
            animator.SetBool("boosting", true);
            boost = boostPower;
            boosting = true;
            engineEffect.Play();
        }
    }

    public void ExitBoost(){
        animator.SetBool("boosting", false);
        boost = 1f;
        boosting = false;
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Obstacle")){
            TakeDamage(1);
        }
    }

    private void TakeDamage(int damage){
        health -= damage;
        UIController.Instance.UpdateHealthSlider(health, maxHealth);
        AudioManager.Instance.PlaySound(AudioManager.Instance.hit);
        spriteRenderer.material = whiteMaterial;
        StartCoroutine("ResetMaterial");
        if (health <= 0){
            boost = 0f;
            gameObject.SetActive(false);
            Instantiate(destroyEffect, transform.position, transform.rotation);
            GameManager.Instance.GameOver();
            AudioManager.Instance.PlaySound(AudioManager.Instance.ice);
        }
    }

    IEnumerator ResetMaterial(){
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.material = defaultMaterial;
    }
}
