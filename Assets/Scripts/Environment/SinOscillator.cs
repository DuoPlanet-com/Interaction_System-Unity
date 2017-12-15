using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public class SinOscillator : MonoBehaviour
    {

        public Vector3 direction;
        public float speed;

        Vector3 startPos;

        private void Start()
        {
            startPos = transform.position;
        }

        private void Update()
        {
            transform.position = new Vector3(
                startPos.x + direction.x * Mathf.Sin(Time.time * speed),
                startPos.y + direction.y * Mathf.Sin(Time.time * speed),
                startPos.z + direction.z * Mathf.Sin(Time.time * speed));
        }

    }
}