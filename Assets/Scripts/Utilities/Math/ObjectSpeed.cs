using System.Collections;
using UnityEngine;

namespace MathCalculation
{
    public class ObjectSpeed : MonoBehaviour
    {
        internal float movingSpeed;
        private Vector3 lastPos;

        private void Start()
        {
            StartCoroutine(CalcVelocity());
        }

        private IEnumerator CalcVelocity()
        {
            while (Application.isPlaying)
            {
                lastPos = gameObject.transform.position;
                yield return new WaitForFixedUpdate();
                movingSpeed = Mathf.RoundToInt(Vector3.Distance(gameObject.transform.position, lastPos) / Time.fixedDeltaTime);
            }
        }
    }
}
