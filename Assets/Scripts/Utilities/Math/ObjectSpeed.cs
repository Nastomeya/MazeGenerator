using System.Collections;
using UnityEngine;

namespace MathCalculation
{
    public class ObjectSpeed : MonoBehaviour
    {
        [SerializeField] private float movingSpeed;
        [SerializeField] private float movingSpeed2;
        internal float MovingSpeed => movingSpeed;
        private Vector3 lastPos;

        private void Start()
        {
            StartCoroutine(CalcVelocity());
            _CalcVelocity();
        }

        private IEnumerator CalcVelocity()
        {
            while (Application.isPlaying)
            {
                lastPos = gameObject.transform.position;
                yield return new WaitForFixedUpdate();

                movingSpeed2 = Mathf.RoundToInt(Vector3.Distance(gameObject.transform.position, lastPos) / Time.fixedDeltaTime);
            }
        }

        private async void _CalcVelocity()
        {
            while (Application.isPlaying)
            {
                movingSpeed = await BasicMath.GetVelocity(gameObject.transform.position);
            }
        }

    }
}
