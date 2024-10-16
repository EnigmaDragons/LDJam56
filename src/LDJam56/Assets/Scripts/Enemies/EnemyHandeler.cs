using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.AI;
using UnityEngine.Events;
using static Unity.Cinemachine.CinemachineTargetGroup;

public class EnemyHandeler : MonoBehaviour
{

    public Transform Target;
    public float HP;
    public float Speed;
    public float Range;
    public float AttackRange;
    public float Attack2Range;
    public float Damage;

    public float AttackDelay;
    public float Attack2Delay;

    public Transform firePoint;
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public bool shoot = false;
    public Transform shootTarget;

    public Collider Collider1;
    public Collider Collider2;

    public UnityEvent OnDeath;
    
    protected bool _targetFound;

    NavMeshAgent agent;
    Animator animator;
    Rigidbody rb;
    private Renderer enemyRenderer;
    private Material enemyMaterial;
    private Color originalColor;
    private Coroutine flashCoroutine;
    private bool _dying = false;

    public float _maxHP;
    public float MaxHp => _maxHP;
    private bool _hasAnimator;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent != null)
        {
            agent.updateRotation = false;
            agent.updateUpAxis = false;
        }

        animator = GetComponent<Animator>();
        _hasAnimator = animator != null;
        rb = GetComponent<Rigidbody>();
        enemyRenderer = GetComponentInChildren<Renderer>();
        _maxHP = HP;
        if (enemyRenderer != null)
        {
            enemyMaterial = new Material(enemyRenderer.material);
            enemyRenderer.material = enemyMaterial;
            originalColor = enemyMaterial.color;
        }
    }
    protected void Update()
    {
        OnUpdate();
    }
    public void BeginAttackBasic1()
    {
        Collider1.enabled = true;
    }
    public void EndAttackBasic1()
    {
        Collider1.enabled = false;
    }
    public void BeginAttackBasic2()
    {
        Collider2.enabled = true;
    }
    public void EndAttackBasic2()
    {
        Collider2.enabled = false;
    }
    public void BasicShoot()
    {
        if (!_targetFound)
            return;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        EnemyBullets script = projectile.GetComponent<EnemyBullets>();
        script.speed = Speed;
        script.target = shootTarget.position;
        Message.Publish(new PlayOneShotSoundEffect(SoundEffectEnum.EnemyShoot, gameObject));//publishing the soundeffect i want to play(need to add to SFX enum) go to fmod soundeffects script add there new stuff
        
        shoot = false;
        agent.updateRotation = true;
    }

    protected virtual void OnUpdate()
    {
        if (_dying)
            return;
        
        if (!_targetFound)
        {
            var obj = GameObject.FindWithTag("Player");
            if (obj != null)
            {
                Target = obj.transform;
                _targetFound = true;
                if (shootTarget == null)
                    shootTarget = Target;
            }
        }
        
        if (HP <= 0)
        {
            _dying = true;
            if (_hasAnimator)
                animator.SetTrigger("death");
            Death();
            OnDeath.Invoke();
            Debug.Log("I died!", this);
            // Disable instead of Destroy to prevent memory churn, and top avoid null hits.
            this.ExecuteAfterDelay(() => this.gameObject.SetActive(false), _hasAnimator ? animator.GetCurrentAnimatorClipInfo(0).Length : 2.5f);
        }
        if(shoot && agent.updateRotation == false)
        {
            Vector3 direction = Target.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            Quaternion smoothedRotation = Quaternion.Slerp(rb.rotation, targetRotation, 5 * Time.deltaTime);
            rb.MoveRotation(smoothedRotation);
        }
    }

    protected virtual void Death()
    {
        Message.Publish(new EnemyKilled());
    }

    public virtual void Damaged(int damage)
    {
        HP -= damage;
        if (HP > 0)
        {
            if (_hasAnimator)
            {
                animator.SetTrigger("hit");
            }
            Message.Publish(new PlayOneShotSoundEffect(SoundEffectEnum.BotDamaged, transform.position));
            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
            }
            flashCoroutine = StartCoroutine(FlashRed());
        }
    }

    private IEnumerator FlashRed()
    {
        float flashDuration = 0.5f;
        float elapsedTime = 0f;

        while (elapsedTime < flashDuration)
        {
            float t = elapsedTime / flashDuration;
            if (enemyMaterial != null)
            {
                enemyMaterial.color = Color.Lerp(Color.red, originalColor, t);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (enemyMaterial != null)
        {
            enemyMaterial.color = originalColor;
        }
    }
}
