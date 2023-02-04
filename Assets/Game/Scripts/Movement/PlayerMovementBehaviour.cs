using UnityEngine;

namespace Game.Scripts.Movement
{
    public class PlayerMovementBehaviour : MonoBehaviour
    {
        [SerializeField] private CharacterMovementBehaviour _cmb;

        public void Initialize()
        {
            _cmb.Initialize();
        }

        public void Update()
        {
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
    }
}
