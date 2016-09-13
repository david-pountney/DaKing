using UnityEngine;
using System.Collections;

public class ParticleCollisionScript : MonoBehaviour {

    void OnParticleCollision(GameObject other)
    {
        Debug.Log(other);
        Rigidbody body = other.GetComponent<Rigidbody>();
        if (body)
        {
            Debug.Log(body);
        }
    }
}
