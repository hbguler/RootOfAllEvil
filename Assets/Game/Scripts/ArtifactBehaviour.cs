using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Weapon;
using UnityEngine;

namespace Game.Scripts
{
    public class ArtifactBehaviour : MonoBehaviour
    {
        public static event Action ArtifactDestroyed;
        [SerializeField] private List<ParticleSystem> _explosionParticles;
        [SerializeField] private GameObject _victoryText;
        [SerializeField] private MeshRenderer _evilMesh;
        
        private int _hp = 1;
        
        private void OnTriggerEnter(Collider collider)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Bullet") ||
             (collider.gameObject.layer == LayerMask.NameToLayer("MeleeWeapon") && collider.gameObject.GetComponent<MeleeWeaponBehaviour>().IsAttacking))
            {
                _hp--;

                if (_hp <= 0)
                {
                    ArtifactDestroyed?.Invoke();

                    foreach (var particle in _explosionParticles)
                    {
                        particle.Play();
                    }

                    StartCoroutine(VictoryCoroutine());
                }
            }
        }

        private IEnumerator VictoryCoroutine()
        {
            yield return new WaitForSeconds(2);
            _evilMesh.enabled = false;
            _victoryText.gameObject.SetActive(true);
        }
    }
}
