using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    
    private void Update()
    {
        transform.Translate(Time.deltaTime * moveSpeed * Vector3.forward);
    }
}
