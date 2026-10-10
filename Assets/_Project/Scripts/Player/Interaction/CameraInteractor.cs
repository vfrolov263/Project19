using System;
using UnityEngine;

namespace Assets._Project.Scripts.Player.Interaction
{
    public class CameraInteractor : IInteractor
    {
        public static CameraInteractor Default => new(Camera.main, rayOffset: new(0.5f, 0.5f));
        private Camera _camera;
        private Vector3 _rayOffset;

        public CameraInteractor(Camera camera, LayerMask mask = default, Vector3 rayOffset = default)
        {
            if (camera == null)
                throw new Exception("Camera is null");

            _camera = camera;
            _rayOffset = rayOffset;
            Mask = mask;
        }

        public Ray Ray => _camera.ViewportPointToRay(_rayOffset);
        public LayerMask Mask { get; private set; }
    }
}