using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    float backgroundImageWidth;

    void Start()
    {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        backgroundImageWidth = sprite.texture.width / sprite.pixelsPerUnit;
    }

    void Update()
    {
        float moveX = moveSpeed * GameManager.Instance.worldSpeed * Time.deltaTime;
        transform.position += new Vector3(moveX, 0);
        if (Mathf.Abs(transform.position.x) - backgroundImageWidth > 0){
            transform.position = new Vector3(0f, transform.position.y);
        }
    }
}
