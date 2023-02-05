using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts
{
    public class ArtifactBehaviour : MonoBehaviour
    {
        public static event Action ArtifactDestroyed;
        [SerializeField] private List<ParticleSystem> _explosionParticles;
        
        private int _hp = 1;
        
        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("MeleeWeapon")||
            collider.gameObject.layer == LayerMask.NameToLayer("Bullet"))
            {
                _hp--;

                if (_hp <= 0)
                {
                    ArtifactDestroyed?.Invoke();

                    foreach (var particle in _explosionParticles)
                    {
                        particle.Play();
                    }
                }
            }

        }
    }
}
