using System;
using UnityEngine;

namespace Game.Scripts
{
    public class ExitTrigger : MonoBehaviour
    {
        public static event Action ExitTriggered;
    
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                ExitTriggered?.Invoke();
            }
        }
    }
}
