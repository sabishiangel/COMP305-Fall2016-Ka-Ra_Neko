using UnityEngine;
using System.Collections;

public class VibeControl : MonoBehaviour {

private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }

        if (other.gameObject.CompareTag("Enemy"))
        {

            Physics2D.IgnoreLayerCollision(3 << LayerMask.NameToLayer("Points"), 4 << LayerMask.NameToLayer("Default"), ignore: true);
        }
    }
}
