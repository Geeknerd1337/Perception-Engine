using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif


namespace Perception.Engine
{
    /// <summary>
    /// This incredibly useful script is thanks to a bit of reading I've done on sound assets in unity from Baruchadi on github. You can see the original gist
    /// this is based on over here:
    /// https://gist.github.com/baruchadi/3c23caf609fa0f4bd349d9ea432eb9c4
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(fileName = "New Sound", menuName = "Perception/Audio/New Sound")]
    public class SoundObject : ScriptableObject
    {
        /// <summary>
        /// Semitones are a unit of measurement for pitch. This is the number of semitones that the pitch of the sound will be shifted by.
        /// This is better than just randoming a pitch because it allows us to have a more consistent sound.
        /// </summary>
        private static readonly float SEMITONES_TO_PITCH_CONVERSION_UNIT = 1.05946f;

        /// <summary>
        /// The number of clips we want to use
        /// </summary>
        public AudioClip[] Clips;

        /// <summary>
        /// The random volume of the sound
        /// </summary>
        [MinMaxSlider(0f, 1f)]
        public Vector2 Volume = new Vector2(0.5f, 0.5f);

        public bool UseSemitones;

        [ShowIf("UseSemitones")]
        [MinMaxSlider(-10, 10)]
        [OnEditorValueChanged("SyncPitchAndSemitones")]
        public Vector2Int Semitones = new Vector2Int(0, 0);

        [HideIf("UseSemitones")]
        [MinMaxSlider(0, 3)]
        [OnEditorValueChanged("SyncPitchAndSemitones")]
        public Vector2 Pitch = new Vector2(1, 1);


        public SoundClipPlayOrder _playOrder;

        public bool Loops = false;
        public SoundRollOffMode RollOffMode = SoundRollOffMode.Linear;
        public float MaxDistance = 500f;
        [Range(0, 128)]
        public int Priority = 0;
        private int _playIndex;



#if UNITY_EDITOR
        private AudioSource _previewer;

        private void OnEnable()
        {
            _previewer = EditorUtility
                .CreateGameObjectWithHideFlags("AudioPreview", HideFlags.HideAndDontSave, typeof(AudioSource))
                .GetComponent<AudioSource>();
        }

        private void OnDisable()
        {
            DestroyImmediate(_previewer.gameObject);
        }

        [Button]
        public void PlayPreview()
        {
            if (_previewer != null)
            {
                _previewer.Stop();
                Play(_previewer, false);
            }
        }

        [Button]
        public void StopPreview()
        {
            if (_previewer != null)
            {
                _previewer.Stop();
            }
        }

#endif
        public AudioSource Play(AudioSource source = null, bool destroy = true)
        {

            if (Clips.Length == 0)
            {
                this.LogWarning($"Missing sound clips for {name}");
                return null;
            }

            if (source == null)
            {
                GameObject obj = new GameObject($"Sound: {name}", typeof(AudioSource));
                source = obj.GetComponent<AudioSource>();
            }

            source.clip = GetAudioClip();
            source.volume = UnityEngine.Random.Range(Volume.x, Volume.y);
            source.pitch = UseSemitones
                ? Mathf.Pow(SEMITONES_TO_PITCH_CONVERSION_UNIT, UnityEngine.Random.Range(Semitones.x, Semitones.y))
                : UnityEngine.Random.Range(Pitch.x, Pitch.y);
            source.loop = Loops;
            source.maxDistance = MaxDistance;
            source.priority = Priority;
            source.rolloffMode = (RollOffMode == SoundRollOffMode.Linear) ? AudioRolloffMode.Linear : AudioRolloffMode.Logarithmic;

            source.Play();

            if (!Loops && destroy)
            {
                Destroy(source.gameObject, source.clip.length / source.pitch);
            }

            return source;
        }

        public void PlayOneShot(AudioSource source)
        {
            source.volume = UnityEngine.Random.Range(Volume.x, Volume.y);
            source.pitch = UnityEngine.Random.Range(Pitch.x, Pitch.y);
            source.loop = Loops;
            source.maxDistance = MaxDistance;
            source.priority = Priority;
            source.PlayOneShot(GetAudioClip());
        }

        public AudioClip GetAudioClip()
        {

            var clip = Clips[_playIndex >= Clips.Length ? 0 : _playIndex];

            switch (_playOrder)
            {
                case SoundClipPlayOrder.Random:
                    _playIndex = UnityEngine.Random.Range(0, Clips.Length);
                    break;
                case SoundClipPlayOrder.Sequential:
                    _playIndex = (_playIndex + 1) % Clips.Length;
                    break;
                case SoundClipPlayOrder.Reverse:
                    _playIndex = (_playIndex + Clips.Length - 1) % Clips.Length;
                    break;
            }

            return clip;
        }

        public void InitializeAudiource(AudioSource original)
        {
            original.clip = GetAudioClip();
            original.volume = Volume.x + UnityEngine.Random.Range(0, Volume.y - Volume.x);
            original.pitch = Pitch.x + UnityEngine.Random.Range(0, Pitch.y - Pitch.x);
            original.loop = Loops;
            original.maxDistance = MaxDistance;
            original.priority = Priority;
        }


        public void SyncPitchAndSemitones()
        {

            if (UseSemitones)
            {
                Pitch.x = Mathf.Pow(SEMITONES_TO_PITCH_CONVERSION_UNIT, Semitones.x);
                Pitch.y = Mathf.Pow(SEMITONES_TO_PITCH_CONVERSION_UNIT, Semitones.y);

            }
            else
            {
                Semitones.x = Mathf.RoundToInt(Mathf.Log10(Pitch.x) / Mathf.Log10(SEMITONES_TO_PITCH_CONVERSION_UNIT));
                Semitones.y = Mathf.RoundToInt(Mathf.Log10(Pitch.y) / Mathf.Log10(SEMITONES_TO_PITCH_CONVERSION_UNIT));

            }


        }




    }

    public enum SoundClipPlayOrder
    {
        Random,
        Sequential,
        Reverse
    }

    public enum SoundRollOffMode
    {
        Linear = 0,
        Logarithmic = 1,
    }
}

