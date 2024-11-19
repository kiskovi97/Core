using UnityEngine;

namespace Kiskovi.Core
{
    public static class WeightedRandom
    {
        public static int GetWeightedRandomIndex(int listLength, float favorLowerIndexes = 0.1f)
        {
            if (listLength <= 0)
            {
                Debug.LogError("List length must be greater than zero.");
                return -1;
            }

            favorLowerIndexes = Mathf.Clamp01(favorLowerIndexes);

            var randomness = 0.5f - Mathf.Abs(favorLowerIndexes - 0.5f);
            var range = listLength * randomness;
            var center = favorLowerIndexes * listLength;
            var min = Mathf.Max(0, center - range);
            var max = Mathf.Min(listLength, center + range);

            return Mathf.FloorToInt(Random.Range(min, max)); // Fallback, should never happen
        }
    }
}
