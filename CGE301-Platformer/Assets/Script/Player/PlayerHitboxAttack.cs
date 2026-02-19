using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHitbox : MonoBehaviour
{
    private Collider2D col;
    private InputAction attack;
    private InputSystem_Actions playerController;

    private AudioSource audioSource;

    [Header("Explosion")]
    [SerializeField] private GameObject bigExplosionPrefab;
    [SerializeField] private Transform explosionSpawnPoint;

    [Header("Combat")]
    [SerializeField] private float stunDuration = 1f;
    [SerializeField] private float hitboxActiveDuration = 0.2f;
    [SerializeField] private float attackCooldown = 2f;

    private Transform playerRoot;
    private float nextAttackTime;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        playerController = new InputSystem_Actions();

        PlayerController owner = GetComponentInParent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        playerRoot = owner != null ? owner.transform : transform.root;

        if (explosionSpawnPoint == null)
        {
            explosionSpawnPoint = FindSpawnPoint(playerRoot);
        }

        if (bigExplosionPrefab == null)
        {
            ParticleSystem fallbackEffect = GetComponentInChildren<ParticleSystem>(true);
            if (fallbackEffect != null)
            {
                bigExplosionPrefab = fallbackEffect.gameObject;
            }
        }
    }

    void Start()
    {
        col.enabled = false;
    }

    void OnEnable()
    {
        attack = playerController.Player.Attack;
        attack.Enable();
    }

    void OnDisable()
    {
        if (attack != null)
        {
            attack.Disable();
        }
    }

    void Update()
    {
        if (!attack.WasPressedThisFrame())
        {
            return;
        }

        if (Time.time < nextAttackTime)
        {
            return;
        }

        nextAttackTime = Time.time + attackCooldown;
        SpawnBigExplosion();
        audioSource.Play();
        StartCoroutine(EnableHitbox(hitboxActiveDuration));
    }

    IEnumerator EnableHitbox(float duration)
    {
        col.enabled = true;
        yield return new WaitForSeconds(duration);
        col.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy"))
        {
            return;
        }

        Debug.Log("Hit Enemy");
        other.gameObject.GetComponent<EnemyDamageHeadler>().StunReceiver(stunDuration);
    }

    private void SpawnBigExplosion()
    {
        if (bigExplosionPrefab == null)
        {
            return;
        }

        Transform spawn = explosionSpawnPoint != null ? explosionSpawnPoint : transform;
        GameObject spawned = Instantiate(bigExplosionPrefab, spawn.position, spawn.rotation);

        ParticleSystem[] systems = spawned.GetComponentsInChildren<ParticleSystem>(true);
        float longestLifetime = 0f;

        for (int i = 0; i < systems.Length; i++)
        {
            ParticleSystem ps = systems[i];
            ps.Play();

            float lifetime = ps.main.duration + ps.main.startLifetime.constantMax;
            if (lifetime > longestLifetime)
            {
                longestLifetime = lifetime;
            }
        }

        Destroy(spawned, longestLifetime > 0f ? longestLifetime + 0.25f : 2f);
    }

    private Transform FindSpawnPoint(Transform root)
    {
        if (root == null)
        {
            return transform;
        }

        string[] pointNames =
        {
            "ExplosionPoint",
            "BigExploPoint",
            "FirePoint",
            "ShootPoint",
            "ShotPoint",
            "Muzzle"
        };

        Transform[] allTransforms = root.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < pointNames.Length; i++)
        {
            for (int j = 0; j < allTransforms.Length; j++)
            {
                if (allTransforms[j].name == pointNames[i])
                {
                    return allTransforms[j];
                }
            }
        }

        return transform;
    }
}
