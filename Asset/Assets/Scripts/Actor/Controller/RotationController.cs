using UnityEngine;

namespace ActorCore
{
    public class RotationController : IActorController
    {

        private Transform _transform;

        public override void Init(Actor actor)
        {
            this.actor = actor;

            _transform = transform;
            target = _transform.rotation;
            angularVelocity = 500f;
        }

        public override void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public Quaternion current
        {
            get
            {
                return _transform.rotation;
            }
            private set
            {
                _transform.rotation = value;
            }
        }

        public Quaternion target
        {
            get; private set;
        }

        public Vector3 currForward
        {
            get
            {
                return _transform.forward;
            }
        }

        public float currAngle
        {
            get
            {
                return current.eulerAngles.y;
            }
        }

        public float currRadian
        {
            get
            {
                return current.eulerAngles.y * Mathf.Deg2Rad;
            }
        }

        public Vector3 targetForward
        {
            get
            {
                return target * Vector3.forward;
            }
        }

        public float targetAngle
        {
            get
            {
                return target.eulerAngles.y;
            }
        }

        public float targetRadian
        {
            get
            {
                return target.eulerAngles.y * Mathf.Deg2Rad;
            }
        }

        public float angularVelocity
        {
            get; set;
        }

        public void To(Vector3 forward, bool bImmediately = false)
        {
            forward.y = 0f;
            if (forward == Vector3.zero) return;
            To(Quaternion.LookRotation(forward), bImmediately);
        }

        public void ToAngle(float angle, bool bImmediately = false)
        {
            To(Quaternion.Euler(0, angle, 0), bImmediately);
        }

        public void ToRadian(float radian, bool bImmediately = false)
        {
            To(Quaternion.Euler(0, radian * Mathf.Rad2Deg, 0), bImmediately);
        }

        public void To(Quaternion to, bool bImmediately = false)
        {
            target = to;

            if (bImmediately)
                current = to;
        }

        public void To(Transform t, bool bImmediately = false)
        {
            Vector3 forward = t.position - _transform.position;
            To(forward, bImmediately);
        }

        void Update()
        {
            if (_transform == null)
                return;

            current = Quaternion.RotateTowards(current, target, angularVelocity * Time.deltaTime);
        }

    }
}