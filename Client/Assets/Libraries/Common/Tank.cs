using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace CommonLib
{
    public class Tank
    {

        private Vector2 _velocity = Vector2.zero;

        private float _angularVelocity = 0f;

        private float _acceleration = 100f;

        private float _angularAcceleration = 3500f;

        private float _linearDamping = 40f;

        private float _angularDamping = 1600f;

        private float _angularVelocityCap = 300;

        private float _linearVelocityCap = 5;

        private Transform _transform;

        public Tank()
        {
            
        }

        public void ApplyControls(PlayerInput controls, float deltaTime)
        {
            _velocity.y += controls.Y * _acceleration * deltaTime;


            if (_velocity.y > _linearVelocityCap)
            {
                _velocity.y = _linearVelocityCap;
            }
            else if (_velocity.y < -_linearVelocityCap)
            {
                _velocity.y = -_linearVelocityCap;
            }

            _angularVelocity -= controls.X * _angularAcceleration * deltaTime;

            if (_angularVelocity > _angularVelocityCap)
            {
                _angularVelocity = _angularVelocityCap;
            }
            else if (_angularVelocity < -_angularVelocityCap)
            {
                _angularVelocity = -_angularVelocityCap;
            }
        }

        public Vector2 CalculatePositionStep(float deltaTime)
        {
            return new Vector2(_velocity.x * deltaTime, _velocity.y * deltaTime);
        }

        public float CalculateRotationStep(float deltaTime)
        {
            return _angularVelocity * deltaTime;
        }

        public void ApplyLinearDrag(float deltaTime)
        {
            if (_velocity.x > 0)
            {
                _velocity.x -= Mathf.Min(_linearDamping * deltaTime, _velocity.x);
            }
            else if (_velocity.x < 0)
            {
                _velocity.x += Mathf.Min(_linearDamping * deltaTime, _velocity.x * -1);
            }

            if (_velocity.y > 0)
            {
                _velocity.y -= Mathf.Min(_linearDamping * deltaTime, _velocity.y);
            }
            else if (_velocity.y < 0)
            {
                _velocity.y += Mathf.Min(_linearDamping * deltaTime, _velocity.y * -1);
            }
        }

        public void ApplyAngularDrag(float deltaTime)
        {
            if (_angularVelocity > 0)
            {
                _angularVelocity -= Mathf.Min(_angularDamping * deltaTime, _angularVelocity);
            }
            else if (_angularVelocity < 0)
            {
                _angularVelocity += Mathf.Min(_angularDamping * deltaTime, _angularVelocity * -1);
            }
        }

        public Transform Transform { get
            {
                return _transform;
            }
        }
    }
}
