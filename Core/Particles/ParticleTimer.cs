using UnityEngine;

namespace Kiskovi.Core
{
    internal class ParticleTimer : MonoBehaviour
    {
        public ParticleSystem _particleSystem;

        private float time;

        public void AddTime(float time)
        {
            this.time = Mathf.Max(this.time, time);
        }

        private void Update()
        {
            if (_particleSystem == null) return;

            if (time > 0)
            {
                time -= Time.deltaTime;
                if (!_particleSystem.isPlaying)
                    _particleSystem.Play();
            } else
            {
                if (_particleSystem.isPlaying)
                    _particleSystem.Stop();
            }
        }
    }
}
