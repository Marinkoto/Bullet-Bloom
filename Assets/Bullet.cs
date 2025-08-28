using Assets;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Bullet : MonoBehaviour
{
    public float speed = 25f;
    public float lifeTime = 5f;
    public ParticleSystem explosion;
    public AudioClip[] explosionSounds;
    public AudioSource source;

    private Vector3 direction;
    private bool directionSet = false;

    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Lock onto the enemy's current position — not the transform
    public void SetTarget(Transform enemyTarget)
    {
        if (enemyTarget != null)
        {
            Vector3 targetPosition = enemyTarget.position;
            direction = (targetPosition - transform.position).normalized;
            directionSet = true;
        }
    }

    void Update()
    {
        if (!directionSet)
        {
            // Default forward if direction not set
            direction = transform.forward;
        }

        transform.position += direction * speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Health enemy))
        {
            AudioManager.Instance.PlaySound(source, explosionSounds[Random.Range(0, explosionSounds.Length)], true);
            Instantiate(explosion, transform.position, Quaternion.identity);
            enemy.TakeDamage(50);
            KillEffect();
        }

        Destroy(gameObject, 0.75f);
    }
    public void KillEffect()
    {
        PostProcessEffects.Instance.SetEffect<LensDistortion>(
            effect => effect.active = true,
            0.2f,
            effect => effect.active = false
            );
        PostProcessEffects.Instance.SetEffect<ChromaticAberration>(
            effect => effect.active = true,
            0.2f,
            effect => effect.active = false
            );
        PostProcessEffects.Instance.SetEffect<Vignette>(
            effect => effect.active = true,
            0.2f,
            effect => effect.active = false
            );
        PostProcessEffects.Instance.SetEffect<Bloom>(
            effect => effect.active = true,
            0.2f,
            effect => effect.active = false
            );
    }
}
