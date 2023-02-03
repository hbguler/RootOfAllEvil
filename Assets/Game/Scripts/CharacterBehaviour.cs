using UnityEngine;

namespace Game.Scripts
{
    public class CharacterBehaviour : MonoBehaviour
    {
        public Animator Animator;
        public int Health;

        public virtual void TakeHit(int damage)
        {
            Health -= damage;

            if (Health <= 0)
            {
                Die();
            }
        }

        public virtual void Die()
        {
            Animator.SetTrigger("Die");
        }
    }
}
