using UnityEngine;

namespace Game.Scripts.Movement
{
    public class PlayerMovementBehaviour : MonoBehaviour
    {
        [SerializeField] private CharacterMovementBehaviour _cmb;

        private bool _canMove;
        public void Initialize()
        {
            _cmb.Initialize();
            _canMove = true;
        }

        public void Update()
        {
            if (_canMove == false)
            {
                _cmb.SetMovementVector(Vector3.zero);
                return;
            }
            
            Vector3 movementVector = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
                movementVector += Vector3.forward;
            if(Input.GetKey(KeyCode.S))
                movementVector += Vector3.back;
            if (Input.GetKey(KeyCode.A))
                movementVector += Vector3.left;
            if(Input.GetKey(KeyCode.D))
                movementVector += Vector3.right;
            
            _cmb.SetMovementVector(movementVector.normalized);
        }

        public void ToggleMovement(bool canMove)
        {
            _canMove = canMove;
        }
    }
}
