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

        [SerializeField]
        private SoundClipPlayOrder _playOrder;

        public bool Loops = false;
        public float MaxDistance = 500f;
        public int Priority = 0;

        private int _playIndex;

        public float f;


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

#endif

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

        enum SoundClipPlayOrder
        {
            Random,
            In_order,
            Reverse
        }


    }

    public enum SoundRollOffMode
    {
        Linear = 0,
        Logarithmic = 1,
    }
}

