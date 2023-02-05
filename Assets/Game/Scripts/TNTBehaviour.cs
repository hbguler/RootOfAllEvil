using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Game.Scripts
{
    public class TNTBehaviour : MonoBehaviour
    {
        [SerializeField] private Collider _trigger;
        
        [SerializeField] private List<GameObject> _tntModels;
        [SerializeField] private List<GameObject> _rocks;

        private Vector3 _target = new Vector3(6, -5.36f, 14.69f);

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
            {
                _trigger.enabled = false;
                Explode();
            }
        }

        private void Explode()
        {
            foreach (GameObject tntModel in _tntModels)
            {
                tntModel.gameObject.SetActive(false);
            }

            foreach (GameObject rock in _rocks)
            {
                rock.transform.DOJump(
                    endValue: _target + UnityEngine.Random.insideUnitSphere,
                    jumpPower: UnityEngine.Random.Range(5f, 15f),
                    numJumps: 1,
                    duration: UnityEngine.Random.Range(0.75f, 1.5f)).OnComplete(() =>
                {
                    rock.gameObject.SetActive(false);
                });
            }
        }
    }
}
