using UnityEngine;

public class LaserBolt : MonoBehaviour
{
    [SerializeField] private float speed = 40f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifetime = 5f;
    private Transform player;
    private LayerMask layermask;
    private GameObject projOwner;


    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var hp = player.GetComponent<hp_system>();
            if (hp != null)
            {
                hp.take_damage(damage);
            }
            Destroy(gameObject);
        }
        else if (other.gameObject == projOwner)
        {
            return;
        }
        else if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }

    public void GiveOwner(GameObject owner)
    {
        projOwner = owner;
    }
}
