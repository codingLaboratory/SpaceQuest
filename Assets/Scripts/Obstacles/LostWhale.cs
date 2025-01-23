using UnityEngine;
using UnityEngine.SceneManagement;

public class LostWhale : MonoBehaviour
{
    void Update()
    {
        float moveX = (GameManager.Instance.worldSpeed * PlayerController.Instance.boost) * Time.deltaTime;
        transform.position += new Vector3(-moveX, 0);
        if (transform.position.x < -11){
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if (collider.gameObject.CompareTag("Player")){
            SceneManager.LoadScene("Level 1 Complete");
        }
    }
}
