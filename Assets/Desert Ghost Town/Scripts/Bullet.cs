using UnityEngine;

namespace Desert_Ghost_Town.Scripts
{
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 5f;

        private void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Destroy(gameObject);
        }
    }
}