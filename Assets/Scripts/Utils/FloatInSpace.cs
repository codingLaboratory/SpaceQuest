using UnityEngine;

public class FloatInSpace : MonoBehaviour
{
    void Update(){
        float moveX = GameManager.Instance.worldSpeed * Time.deltaTime;
        transform.position += new Vector3(-moveX, 0);
    }
}
