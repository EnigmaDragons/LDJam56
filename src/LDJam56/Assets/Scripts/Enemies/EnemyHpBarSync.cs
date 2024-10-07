using UnityEngine;
using Renge.PPB;

public class EnemyHpBarSync : MonoBehaviour
{
    [SerializeField] private EnemyHandeler enemyHandler;
    [SerializeField] private ProceduralProgressBar hpBar;

    private bool _isInitialized = false;
    private bool _isProgressBarInitialized = false;

    private void Start()
    {
        if (enemyHandler == null)
        {
            Debug.LogError("EnemyHandler component not found on this GameObject.");
        }

        if (hpBar == null)
        {
            Debug.LogError("ProceduralProgressBar component not found in children.");
        }
        _isInitialized = true;
    }

    private void Update()
    {
        if (!_isInitialized) return;

        if (!_isProgressBarInitialized)
        {
            _isProgressBarInitialized = hpBar.IsInitialized;
            return;
        }
        
        var currentHp = enemyHandler.HP;
        var maxHp = enemyHandler.MaxHp;
        
        // Update the progress bar
        hpBar.SegmentCount = maxHp;
        hpBar.Value = currentHp;
        
        if (currentHp <= 0)
            hpBar.gameObject.SetActive(false);
    }
}
