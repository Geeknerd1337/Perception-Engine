using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Perception.Engine
{
    public class AudioReverbData
    {
        public AudioReverbData()
        {

        }
        public AudioReverbData(AudioReverbData a)
        {
            hfReference = a.hfReference;
            density = a.density;
            diffusion = a.diffusion;
            reverbDelay = a.reverbDelay;
            reverb = a.reverb;
            reflectDelay = a.reflectDelay;
            reflections = a.reflections;
            decayHFRatio = a.decayHFRatio;
            decayTime = a.decayTime;
            roomHF = a.roomHF;
            room = a.room;
            roomLF = a.roomLF;
            lfReference = a.lfReference;
        }
        public AudioReverbData(AudioReverbZone a)
        {
            hfReference = a.HFReference;
            density = a.density;
            diffusion = a.diffusion;
            reverbDelay = a.reverbDelay;
            reverb = a.reverb;
            reflectDelay = a.reflectionsDelay;
            reflections = a.reflections;
            decayHFRatio = a.decayHFRatio;
            decayTime = a.decayTime;
            roomHF = a.roomHF;
            room = a.room;
            roomLF = a.roomLF;
            lfReference = a.LFReference;
            //Print all these values with their names
            //Debug.Log( $"HFReference: {hfReference} Density: {density} Diffusion: {diffusion} ReverbDelay: {reverbDelay} Reverb: {reverb} ReflectDelay: {reflectDelay} Reflections: {reflections} DecayHFRatio: {decayHFRatio} DecayTime: {decayTime} RoomHF: {roomHF} Room: {room} RoomLF: {roomLF} LFReference: {lfReference}" );

        }

        public AudioReverbData(AudioReverbProfile a)
        {
            hfReference = a.HFReference;
            density = a.density;
            diffusion = a.diffusion;
            reverbDelay = a.reverbDelay;
            reverb = a.reverb;
            reflectDelay = a.reflectionsDelay;
            reflections = a.reflections;
            decayHFRatio = a.decayHFRatio;
            decayTime = a.decayTime;
            roomHF = a.roomHF;
            room = a.room;
            roomLF = a.roomLF;
            lfReference = a.LFReference;
            //Print all these values with their names
            Debug.Log($"HFReference: {hfReference} Density: {density} Diffusion: {diffusion} ReverbDelay: {reverbDelay} Reverb: {reverb} ReflectDelay: {reflectDelay} Reflections: {reflections} DecayHFRatio: {decayHFRatio} DecayTime: {decayTime} RoomHF: {roomHF} Room: {room} RoomLF: {roomLF} LFReference: {lfReference}");

        }
        public int priority;
        public Collider shape;
        public float fadeDistance;
        public float hfReference;
        public float density;
        public float diffusion;
        public float reverbDelay;
        public float reverb;
        public float reflectDelay;
        public float reflections;
        public float decayHFRatio;
        public float decayTime;
        public float roomHF;
        public float room;
        public float roomLF;
        public float lfReference;
        public static AudioReverbData Lerp(AudioReverbData a, AudioReverbData b, float t)
        {
            AudioReverbData c = new AudioReverbData();
            c.hfReference = Mathf.Lerp(a.hfReference, b.hfReference, t);
            c.density = Mathf.Lerp(a.density, b.density, t);
            c.diffusion = Mathf.Lerp(a.diffusion, b.diffusion, t);
            c.reverbDelay = Mathf.Lerp(a.reverbDelay, b.reverbDelay, t);
            c.reverb = Mathf.Lerp(a.reverb, b.reverb, t);
            c.reflectDelay = Mathf.Lerp(a.reflectDelay, b.reflectDelay, t);
            c.reflections = Mathf.Lerp(a.reflections, b.reflections, t);
            c.decayHFRatio = Mathf.Lerp(a.decayHFRatio, b.decayHFRatio, t);
            c.decayTime = Mathf.Lerp(a.decayTime, b.decayTime, t);
            c.roomHF = Mathf.Lerp(a.roomHF, b.roomHF, t);
            c.room = Mathf.Lerp(a.room, b.room, t);
            c.roomLF = Mathf.Lerp(a.roomLF, b.roomLF, t);
            c.lfReference = Mathf.Lerp(a.lfReference, b.lfReference, t);
            return c;
        }
    }
    public class AudioReverbConsumer : MonoBehaviour
    {
        public LayerMask reverbLayer;
        public AudioMixer target;
        public AudioReverbData defaultSettings;
        private void Start()
        {
            AudioReverbZone zone = gameObject.AddComponent<AudioReverbZone>();
            zone.reverbPreset = AudioReverbPreset.City;
            defaultSettings = new AudioReverbData(zone);
            DestroyImmediate(zone);


            bool check = true;
            check = check && target.GetFloat("HF Reference", out defaultSettings.hfReference);
            check = check && target.GetFloat("Density", out defaultSettings.density);
            check = check && target.GetFloat("Diffusion", out defaultSettings.diffusion);
            check = check && target.GetFloat("Reverb Delay", out defaultSettings.reverbDelay);
            check = check && target.GetFloat("Reflections", out defaultSettings.reflections);
            check = check && target.GetFloat("Decay HF Ratio", out defaultSettings.decayHFRatio);
            check = check && target.GetFloat("Decay Time", out defaultSettings.decayTime);
            check = check && target.GetFloat("Room HF", out defaultSettings.roomLF);
            check = check && target.GetFloat("Room", out defaultSettings.room);
            check = check && target.GetFloat("Room LF", out defaultSettings.roomLF);
            check = check && target.GetFloat("LF Reference", out defaultSettings.lfReference);
            if (!check)
            {
                throw new UnityException("Audio reverb variables need to be exposed within the target mixer! (They are not.)");
            }
        }


        Collider[] colliders = new Collider[32];


        void FixedUpdate()
        {
            //Get data using the default settings
            AudioReverbData data = new AudioReverbData(defaultSettings);

            //Create a list of colliders that are in the reverb layer
            List<AudioReverbData> l = new List<AudioReverbData>();

            //Store them into those colliders
            Physics.OverlapSphereNonAlloc(transform.position, 50f, colliders, reverbLayer, QueryTriggerInteraction.Collide);



            //Iterate over each collider
            foreach (Collider c in colliders)
            {
                if (c == null)
                    break;
                //See if it has an audio reverb data
                AudioReverbArea d = c.GetComponent<AudioReverbArea>();

                if (d == null)
                    continue;
                //Add it to a list
                l.Add(d.data);
            }

            //Sort the list by priority
            l.Sort((a, b) => a.priority.CompareTo(b.priority));

            //Lerp the data by the fade distance
            foreach (AudioReverbData d in l)
            {
                Vector3 closestPoint = d.shape.ClosestPoint(transform.position);
                float dist = Vector3.Distance(closestPoint, transform.position);
                data = AudioReverbData.Lerp(data, d, Mathf.Clamp01((d.fadeDistance - dist) / d.fadeDistance));
            }

            target.SetFloat("HF Reference", data.hfReference);
            target.SetFloat("Density", data.density);
            target.SetFloat("Diffusion", data.diffusion);
            target.SetFloat("Reverb Delay", data.reverbDelay);
            target.SetFloat("Reverb", data.reverb);
            target.SetFloat("Reflect Delay", data.reflectDelay);
            target.SetFloat("Reflections", data.reflections);
            target.SetFloat("Decay HF Ratio", data.decayHFRatio);
            target.SetFloat("Decay Time", data.decayTime);
            target.SetFloat("Room HF", data.roomHF);
            target.SetFloat("Room", data.room);
            //target.SetFloat("Dry Level", data);
            target.SetFloat("Room LF", data.roomLF);
            target.SetFloat("LF Reference", data.lfReference);
        }
    }
}
