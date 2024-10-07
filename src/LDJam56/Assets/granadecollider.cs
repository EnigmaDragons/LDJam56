using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class granadecollider : MonoBehaviour
{
    public GameObject boom;
    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(boom, transform.position, Quaternion.identity);
    }
}
