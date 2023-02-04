using System;
using DG.Tweening;
using UnityEngine;

namespace Game.Scripts
{
    public class BridgeBehaviour : MonoBehaviour
    {
        [SerializeField] private Vector3 _closedScale = new Vector3(0, 1, 1);
        [SerializeField] private Vector3 _openScale = Vector3.one;
        [SerializeField] private GameObject _bridge;

        [SerializeField] private BridgeButtonBehaviour _bridgeButton;

        private void Start()
        {
            Initialize();
        }

        public void Initialize()
        {
            _bridgeButton.Initialize();
            _bridge.transform.localScale = _closedScale;

            _bridgeButton.ButtonPressed += OnButtonPressed;
        }

        public void OnButtonPressed()
        {
            _bridgeButton.ButtonPressed -= OnButtonPressed;
            OpenBridge();

        }

        public void OpenBridge()
        {
            _bridge.transform.DOScale(_openScale, 1f).SetEase(Ease.InOutQuad);
        }
    }
}
