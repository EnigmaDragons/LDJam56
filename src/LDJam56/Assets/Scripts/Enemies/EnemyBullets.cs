using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullets : MonoBehaviour
{
    public float speed;
    public Vector3 target;
    Vector3 actualTarget = Vector3.zero;//to lazy to fiagure it out right now
    bool flag;
    Vector3 direction;
    private void Awake()
    {
        flag = true;
        Destroy(gameObject, 7f);
    }
    private void Update()
    {
        if (flag)
        {
            actualTarget = target;
            direction = (actualTarget - transform.position).normalized;
            flag = false;
        }
        
        transform.position += direction * speed * Time.deltaTime;
    }
}
    
        

