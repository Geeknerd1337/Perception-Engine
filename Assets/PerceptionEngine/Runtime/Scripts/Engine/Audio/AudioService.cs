using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Perception.Engine
{
    public class AudioService : PerceptionService
    {
        public IEnumerator FadeTo(AudioSource audioSource, float FadeTime, float TargetVolume)
        {
            float startVolume = audioSource.volume;

            for (float t = 0.0f; t < FadeTime; t += Time.deltaTime)
            {
                if (audioSource == null)
                {
                    yield break;
                }
                audioSource.volume = Mathf.Lerp(startVolume, TargetVolume, t / FadeTime);
                yield return null;
            }

            audioSource.volume = TargetVolume;
        }
    }
}
