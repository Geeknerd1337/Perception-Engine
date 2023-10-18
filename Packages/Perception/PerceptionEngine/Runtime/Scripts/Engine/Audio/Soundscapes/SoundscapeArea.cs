using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Perception.Engine
{
    public class SoundscapeArea : MonoBehaviour
    {
        [Range(0.0f, 1.0f)]
        public float MasterVolume = 1.0f;

        public List<SoundscapeSoundSource> SoundSources = new List<SoundscapeSoundSource>();

        public SoundscapeProfile Profile;

        public Collider MyCollider;

        private AudioListener MyListener;

        public float FadeDistance = 5.0f;

        public void Start()
        {

            MyCollider = GetComponent<Collider>();
            foreach (SoundscapeSoundObject sound in Profile.Sounds)
            {
                if (sound.AffectedByReverb)
                {
                    AudioSource screenSource = AssetService.GetAudioSource("Screen With Reverb");
                    var _obj = Object.Instantiate(screenSource);
                    _obj.transform.SetParent(transform);
                    _obj.transform.localPosition = Vector3.zero;
                    _obj.name = sound.name;
                    sound.InitializeAudiource(_obj);
                    AudioSource _a = null;
                    if (sound.Loops)
                    {
                        _a = sound.Play(_obj.GetComponent<AudioSource>());
                    }

                    SoundscapeSoundSource _soundScapeSource = _obj.gameObject.AddComponent<SoundscapeSoundSource>();
                    _soundScapeSource.SoundScapeSound = sound;
                    _soundScapeSource.AudioSource = _obj.GetComponent<AudioSource>();


                    SoundSources.Add(_soundScapeSource);

                }
                else
                {
                    AudioSource screenSource = AssetService.GetAudioSource("Screen");
                    var _obj = Object.Instantiate(screenSource);
                    _obj.transform.SetParent(transform);
                    _obj.transform.localPosition = Vector3.zero;
                    _obj.name = sound.name;
                    sound.InitializeAudiource(_obj);
                    AudioSource _a = null;
                    if (sound.Loops)
                    {
                        _a = sound.Play(_obj.GetComponent<AudioSource>());
                    }


                    SoundscapeSoundSource _soundScapeSource = _obj.gameObject.AddComponent<SoundscapeSoundSource>();
                    _soundScapeSource.SoundScapeSound = sound;
                    _soundScapeSource.AudioSource = _obj.GetComponent<AudioSource>();



                    SoundSources.Add(_soundScapeSource);
                }
            }
        }

        public void Update()
        {
            foreach (SoundscapeSoundSource soundSource in SoundSources)
            {
                soundSource.SetVolume(MasterVolume);

            }

            foreach (SoundscapeSoundSource soundSource in SoundSources)
            {
                //Doing this here since audio listener isn't set in start
                if (MyListener == null)
                {
                    MyListener = GameManager.GetService<CameraService>().Camera.GetComponent<AudioListener>();
                }


                //Set volume based on the closest point to the listener
                var _closest = MyCollider.ClosestPoint(MyListener.transform.position);
                var _distance = Vector3.Distance(_closest, MyListener.transform.position);

                //Set volume based on linear distance
                soundSource.SetVolume(MasterVolume * (1.0f - Mathf.Clamp((_distance / FadeDistance), 0f, 1f)));
            }
        }

    }

    public class SoundscapeSoundSource : MonoBehaviour
    {
        public TimeSince TimeSinceStarted;
        private float _timeValue;
        public SoundscapeSoundObject SoundScapeSound;
        public AudioSource AudioSource;

        private float _volume;
        private float _baseVolumeValue;

        void Start()
        {
            AudioSource = GetComponent<AudioSource>();
            _baseVolumeValue = AudioSource.volume;
            _volume = _baseVolumeValue;
            //Add random phase to the _timeValue
            _timeValue = Random.Range(SoundScapeSound.PlayInterval.x, SoundScapeSound.PlayInterval.y) + Random.Range(0f, 10f);
        }

        void Update()
        {
            if (TimeSinceStarted > _timeValue)
            {
                if (!AudioSource.loop)
                {
                    AudioSource.Play();
                    TimeSinceStarted = 0f;
                    _timeValue = Random.Range(SoundScapeSound.PlayInterval.x, SoundScapeSound.PlayInterval.y);
                }
            }
        }

        public void Play()
        {
            var _src = SoundScapeSound.Play(AudioSource);
            //Lower it by the requested amount
            _baseVolumeValue = _src.volume;
            SetVolume(_volume);
        }

        public void SetVolume(float f)
        {
            _volume = f;
            AudioSource.volume = _baseVolumeValue * _volume;
        }


    }
}
