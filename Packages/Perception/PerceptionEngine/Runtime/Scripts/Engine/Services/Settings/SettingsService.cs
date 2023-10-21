using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

using UnityEngine.Audio;


namespace Perception.Engine
{
    public class SettingsService : PerceptionService
    {
        /// <summary>
        /// Public reference to our data
        /// </summary>
        public static SettingsData Data => _data;

        /// <summary>
        /// Private instance of our data object
        /// </summary>
        private static SettingsData _data;

        /// <summary>
        /// The path and file name of our settings file
        /// </summary>
        private string SettingsPath => $"{Application.persistentDataPath}/settings";
        private string SettingsFileName => "settings.json";

        private AudioMixer _mixer;

        public override void Awake()
        {
            base.Awake();
            _data = new SettingsData();
        }

        public virtual void Start()
        {
            Load(SettingsFileName);
            SetSettings();
        }

        public void SetData(SettingsData data)
        {
            _data = data.DeepCopy();
        }

        public void SaveSettings()
        {
            Save(_data, SettingsFileName);
            SetSettings();
        }

        public virtual void SetSettings()
        {
            //Load the MainMixer audio mixer from the resource folder
            _mixer = Resources.Load<AudioMixer>("Perception/Audio/MainMixer");

            //Logorithimally set the volume
            _mixer.SetFloat("SFXvolume", Mathf.Log10(_data.MasterVolume) * 20);
            _mixer.SetFloat("MusicVolume", Mathf.Log10(_data.MusicVolume) * 20);

            //Set the resolution to full screen
            Screen.SetResolution(_data.ResolutionWidth, _data.ResolutionHeight, _data.Fullscreen);

            //Set the quality level
            QualitySettings.SetQualityLevel(_data.QualityLevel);

            //Set VSync to be true
            QualitySettings.vSyncCount = _data.VSync ? 1 : 0;
        }


        public virtual void Save(SettingsData data, string fileName)
        {
            if (!Directory.Exists(SettingsPath))
            {
                Directory.CreateDirectory(SettingsPath);
            }
            var stringData = JsonUtility.ToJson(data);
            string path = SettingsPath + "/" + fileName;
            StreamWriter writer = new StreamWriter(path, false);
            writer.Write(stringData);
            writer.Close();
        }

        public virtual void Load(string fileName)
        {
            //If the file doesn't exit, return 
            if (!File.Exists($"{SettingsPath}/{fileName}"))
            {
                return;
            }

            this.Log("Loading settings: " + $"{SettingsPath}/{fileName}");


            //Read the data from the file
            _data = JsonUtility.FromJson<SettingsData>(File.ReadAllText($"{SettingsPath}/{fileName}"));
        }
    }

    [Serializable]
    public class SettingsData
    {
        //General
        public float LookSensitivity = 50f;
        public bool InvertYAxis = false;
        public float FieldOfView = 68f;
        public float ScreenShake = 1f;

        //Graphics
        public int ResolutionWidth = 1920;
        public int ResolutionHeight = 1080;
        public bool Fullscreen = true;
        public bool VSync = true;
        public int QualityLevel = 5;

        //Audio
        public float MasterVolume = 1f;
        public float MusicVolume = 1f;
        public bool Subtitles = false;

        public Dictionary<string, object> CustomSettings = new Dictionary<string, object>();

        public SettingsData DeepCopy()
        {
            return new SettingsData()
            {
                LookSensitivity = LookSensitivity,
                InvertYAxis = InvertYAxis,
                FieldOfView = FieldOfView,
                ScreenShake = ScreenShake,
                ResolutionWidth = ResolutionWidth,
                ResolutionHeight = ResolutionHeight,
                Fullscreen = Fullscreen,
                VSync = VSync,
                QualityLevel = QualityLevel,
                MasterVolume = MasterVolume,
                MusicVolume = MusicVolume,
                Subtitles = Subtitles
            };
        }

        //A boolean to check if the settings have been changed
        public bool SettingsChanged(SettingsData data)
        {
            return LookSensitivity != data.LookSensitivity ||
                   InvertYAxis != data.InvertYAxis ||
                   FieldOfView != data.FieldOfView ||
                   ScreenShake != data.ScreenShake ||
                   ResolutionWidth != data.ResolutionWidth ||
                   ResolutionHeight != data.ResolutionHeight ||
                   Fullscreen != data.Fullscreen ||
                   VSync != data.VSync ||
                   QualityLevel != data.QualityLevel ||
                   MasterVolume != data.MasterVolume ||
                   MusicVolume != data.MusicVolume ||
                   Subtitles != data.Subtitles;
        }

    }

}
