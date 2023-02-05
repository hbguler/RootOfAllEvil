using System;
using UnityEngine;

namespace Game.Scripts
{
    public class ArtifactBehaviour : MonoBehaviour
    {
        public static event Action ArtifactDestroyed;
        
        private int _hp = 3;
        
        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("MeleeWeapon")||
            collider.gameObject.layer == LayerMask.NameToLayer("Bullet"))
            {
                _hp--;

                if (_hp <= 0)
                {
                    ArtifactDestroyed?.Invoke();
                }
            }

        }
    }
}
