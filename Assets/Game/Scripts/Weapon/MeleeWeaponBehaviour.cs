using System.Collections;
using UnityEngine;

namespace Game.Scripts.Weapon
{
    public class MeleeWeaponBehaviour : WeaponBehaviour
    {
        public bool IsAttacking;
        
        public override void Attack(Vector3 direction)
        {
            base.Attack(direction);
            IsAttacking = true;
            StartCoroutine(Wearout());
        }

        private IEnumerator Wearout()
        {
            yield return new WaitForSeconds(GameConfig.MeleeAttackInterval);
            IsAttacking = false;
        }
    }
}
