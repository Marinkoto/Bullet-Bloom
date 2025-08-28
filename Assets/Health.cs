using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets
{
    public class Health : MonoBehaviour, IDamagable
    {
        [SerializeField] int health;
        [SerializeField] int maxHealth;
        private void Start()
        {
            health = maxHealth;
        }
        public void TakeDamage(int amount)
        {
            health -= amount;
            if(health <= 0)
            {
                Die();
            }
        }
        public virtual void Die()
        {
            Destroy(gameObject);
            GameManager.Instance.AddScore();
        }
    }
}
