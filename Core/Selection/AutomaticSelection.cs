using UnityEngine;

namespace Kiskovi.Core
{
    internal class AutomaticSelection : SelectableBase
    {
        public SelectableGameplayElement gameplayElement;

        public override bool CanBeSelected => base.CanBeSelected && gameplayElement.Accesable && isVisibleInsideCamera;
        public override int Priority => gameplayElement.Priority;

        private bool isVisibleInsideCamera = true;

        protected override void Update()
        {
            base.Update();
            isVisibleInsideCamera = IsVisibleFromCamera(Camera.main);
        }

        private bool IsVisibleFromCamera(Camera camera)
        {
            if (camera == null) return false;
            Vector3 viewportPosition = camera.WorldToViewportPoint(transform.position);

            if (viewportPosition.x >= 0 && viewportPosition.x <= 1 &&
                viewportPosition.y >= 0 && viewportPosition.y <= 1 &&
                viewportPosition.z > 0)
            {
                if (camera.projectionMatrix.m11 == 0.0f)
                {
                    return true;
                }
                else
                {
                    return viewportPosition.z <= camera.farClipPlane && viewportPosition.z >= camera.nearClipPlane;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
