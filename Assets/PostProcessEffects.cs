using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;

public class PostProcessEffects : Singleton<PostProcessEffects> 
{

    [SerializeField] private Volume postProcessVolume;

    private Dictionary<string, Coroutine> activeResets = new Dictionary<string, Coroutine>();

    /// <summary>
    /// Set a float property on a post-processing effect with auto-reset after duration.
    /// </summary>
    /// <typeparam name="T">VolumeComponent type (e.g. Bloom)</typeparam>
    /// <param name="propertySetter">Action to set the value</param>
    /// <param name="resetValue">Value to reset to</param>
    /// <param name="value">Value to set temporarily</param>
    /// <param name="duration">Time in seconds before reset</param>
    public void SetEffect<T>(Action<T> propertySetter, float duration, Action<T> resetValue = null) where T : VolumeComponent
    {
        if (postProcessVolume.profile.TryGet(out T effect))
        {
            string key = typeof(T).Name;

            if (activeResets.TryGetValue(key, out var existing))
            {
                StopCoroutine(existing);
            }

            propertySetter(effect);
            effect.active = true;

            Coroutine resetRoutine = StartCoroutine(ResetEffectAfterDelay(effect, resetValue, duration));
            activeResets[key] = resetRoutine;
        }
        else
        {
            Debug.LogWarning($"Effect {typeof(T).Name} not found on Volume.");
        }
    }

    private IEnumerator ResetEffectAfterDelay<T>(T effect, Action<T> resetValue, float duration) where T : VolumeComponent
    {
        yield return new WaitForSeconds(duration);

        resetValue?.Invoke(effect);
        effect.active = false;

        activeResets.Remove(typeof(T).Name);
    }
}
