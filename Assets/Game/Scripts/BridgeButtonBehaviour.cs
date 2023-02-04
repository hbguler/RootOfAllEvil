using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Scripts
{
    public class BridgeButtonBehaviour : MonoBehaviour
    {
        public event Action ButtonPressed;
        
        [SerializeField] private GameObject _button;
        private bool _isPressed;
        
        public void Initialize()
        {
            _isPressed = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isPressed)
                return;
            
            if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
            {
                _isPressed = true;
                PressButton();
                
                other.gameObject.GetComponent<BulletBehaviour>().Kill();
            }
        }

        private void PressButton()
        {
            _button.transform.DOMoveX(_button.transform.position.x - 0.1f, 0.5f).SetLoops(2, LoopType.Yoyo).OnComplete(
                () =>
                {
                    ButtonPressed?.Invoke();
                });
        }
    }
}
