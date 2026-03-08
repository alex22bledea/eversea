using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    [SerializeField] private float lifeTime = 1f;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
