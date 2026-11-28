
using System;
using UnityEngine;

public class EnemyState : MonoBehaviour, IEntityState
{
    [SerializeField] private float maxHealth = 5f;
    [SerializeField] private float currentHealth = 5f;
    [SerializeField] private bool isDead;

    [Serializable]
    private class SaveData
    {
        public float currentHealth;
        public bool isDead;
    }

    public string CaptureState()
    {
        SaveData data = new SaveData
        {
            currentHealth = currentHealth,
            isDead = isDead
        };

        return JsonUtility.ToJson(data);
    }

    public void RestoreState(string json)
    {
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        if (data == null)
            return;

        currentHealth = data.currentHealth;
        isDead = data.isDead;

        // Sau này có thể gọi EnemyHealth hoặc EnemyAI
        // để cập nhật animation và hành vi tương ứng.
    }

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;

        currentHealth = Mathf.Max(0, currentHealth - damage);

        if (currentHealth <= 0)
        {
            isDead = true;
        }
    }
}
