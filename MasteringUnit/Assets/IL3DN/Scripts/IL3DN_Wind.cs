using UnityEngine;

namespace IL3DN
{
    [ExecuteInEditMode]
    public class IL3DnWind : MonoBehaviour
    {
        private readonly float _windGizmo = 0.5f;

        private void Update()
        {
            Shader.SetGlobalVector("WindDirection", transform.rotation * Vector3.back);
        }

        private void OnDrawGizmos()
        {
            var dir = (transform.position + transform.forward).normalized;

            Gizmos.color = Color.green;
            var up = transform.up;
            var side = transform.right;

            var end = transform.position + transform.forward * (_windGizmo * 5f);
            var mid = transform.position + transform.forward * (_windGizmo * 2.5f);
            var start = transform.position + transform.forward * (_windGizmo * 0f);

            var s = _windGizmo;
            var front = transform.forward * _windGizmo;

            Gizmos.DrawLine(start, start - front + up * s);
            Gizmos.DrawLine(start, start - front - up * s);
            Gizmos.DrawLine(start, start - front + side * s);
            Gizmos.DrawLine(start, start - front - side * s);
            Gizmos.DrawLine(start, start - front * 2);

            Gizmos.DrawLine(mid, mid - front + up * s);
            Gizmos.DrawLine(mid, mid - front - up * s);
            Gizmos.DrawLine(mid, mid - front + side * s);
            Gizmos.DrawLine(mid, mid - front - side * s);
            Gizmos.DrawLine(mid, mid - front * 2);

            Gizmos.DrawLine(end, end - front + up * s);
            Gizmos.DrawLine(end, end - front - up * s);
            Gizmos.DrawLine(end, end - front + side * s);
            Gizmos.DrawLine(end, end - front - side * s);
            Gizmos.DrawLine(end, end - front * 2);
        }
    }
}