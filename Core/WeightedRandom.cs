using UnityEngine;

namespace Kiskovi.Core
{
    public static class WeightedRandom
    {
        public static int GetWeightedRandomIndex(int listLength, float random = 0.1f)
        {
            if (listLength <= 0)
            {
                Debug.LogError("List length must be greater than zero.");
                return -1;
            }

            var clampedRandom = Mathf.Clamp01(random);

            var center = Random.Range(0, listLength * clampedRandom);
            var range = Random.Range(0, listLength / 2 * clampedRandom);


            var min = Mathf.Max(0, center - range);
            var max = Mathf.Min(listLength, center + range);

            return Mathf.FloorToInt(Random.Range(min, max)); // Fallback, should never happen
        }
    }
}
