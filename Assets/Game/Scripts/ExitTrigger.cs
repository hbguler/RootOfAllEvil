using System;
using UnityEngine;

namespace Game.Scripts
{
    public class ExitTrigger : MonoBehaviour
    {
        public static event Action ExitTriggered;

        [SerializeField] private ParticleSystem _portalPS;
        private bool _isTriggered = false;
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player") && _isTriggered == false)
            {
                _isTriggered = true;
                _portalPS.Play();
                
                ExitTriggered?.Invoke();
            }
        }
    }
}
