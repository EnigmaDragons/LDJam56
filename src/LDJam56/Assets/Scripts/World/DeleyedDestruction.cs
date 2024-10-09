using UnityEngine;

public class DeleyedDestruction : MonoBehaviour
{
    [SerializeField] private float seconds;

    private float _t = 0f;

    private void Update()
    {
        _t += Time.deltaTime;

        if (_t >= seconds)
            Destroy(gameObject);
    }
}