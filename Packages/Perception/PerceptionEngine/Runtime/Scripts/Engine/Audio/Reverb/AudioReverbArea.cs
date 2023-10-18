using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


namespace Perception.Engine
{
    public class AudioReverbArea : MonoBehaviour
    {

        // Static so you can control all of them from any component.
        // Set in OnValidate
        public bool drawGizmos;
        public static bool drawFade = true;

        /// <summary>
        /// Default parameters for the reverb area
        /// </summary>
        public AudioReverbPreset preset;

        /// <summary>
        /// How far the fade distance is for the reverb area
        /// </summary>
        [Range(0.01f, 10f)]
        public float fadeDistance = 1f;

        /// <summary>
        /// Reference to the audio reverb data associated with this reverb area
        /// </summary>
        [HideInInspector]
        public AudioReverbData data;

        /// <summary>
        /// Optional profile for additional control
        /// </summary>
        public AudioReverbProfile Profile;

        /// <summary>
        /// The priority of this audio reverb area
        /// </summary>
        public int priority = 0;

        [SerializeField, HideInInspector]
        protected Collider shape;

        protected virtual Color gizmoColour => new Color(1, 0.75f, 0, 0.7f);
        public bool drawGizmo;

        private void Start()
        {
            //Create a zone based on the preset
            AudioReverbZone zone = gameObject.AddComponent<AudioReverbZone>();
            zone.reverbPreset = preset;
            zone.minDistance = 0f;
            zone.maxDistance = 0f;

            //Debug.Log( preset.ToString() );

            //Set the data to the reverb zone preset
            data = new AudioReverbData(zone);

            //If profile isn't null, use that instead
            if (Profile != null)
            {
                data = new AudioReverbData(Profile);
            }

            data.priority = priority;
            data.shape = shape;
            data.fadeDistance = fadeDistance;

            //Destroy the zone (is this neccessary?)
            Destroy(zone);
        }

        protected virtual void OnValidate()
        {
            // Collider
            if (!shape)
                shape = GetComponent<Collider>();

            if (shape)
                shape.isTrigger = true;


        }

        protected virtual void OnDrawGizmos()
        {
            if (!drawGizmos || !shape)
                return;

            Matrix4x4 originalMatrix = Gizmos.matrix;

            Gizmos.matrix = transform.localToWorldMatrix;

            Gizmos.color = gizmoColour;

            if (shape is BoxCollider boxCollider)
            {
                Gizmos.DrawWireCube(boxCollider.center, boxCollider.size);

                Gizmos.color *= new Color(1, 1, 1, 0.5f);
                Gizmos.DrawCube(boxCollider.center, boxCollider.size);

                if (drawFade)
                {
                    Gizmos.DrawWireCube(boxCollider.center, boxCollider.size + (Vector3.one * fadeDistance));

                    DrawCubeCornerLine(boxCollider.size, Vector3.one);
                    DrawCubeCornerLine(boxCollider.size.FlipX(), Vector3.one.FlipX());
                    DrawCubeCornerLine(boxCollider.size.FlipY(), Vector3.one.FlipY());
                    DrawCubeCornerLine(boxCollider.size.FlipZ(), Vector3.one.FlipZ());
                }

                void DrawCubeCornerLine(Vector3 size, Vector3 direction)
                {
                    Gizmos.DrawLine(boxCollider.center + size / 2, boxCollider.center + (size / 2) + (direction * fadeDistance / 2));
                    Gizmos.DrawLine(boxCollider.center - size / 2, boxCollider.center - (size / 2) - (direction * fadeDistance / 2));
                }
            }
            else if (shape is SphereCollider sphereCollider)
            {
                Gizmos.DrawWireSphere(sphereCollider.center, sphereCollider.radius);

                Gizmos.color *= new Color(1, 1, 1, 0.5f);
                Gizmos.DrawSphere(sphereCollider.center, sphereCollider.radius);

                if (drawFade)
                {
                    Gizmos.DrawWireSphere(sphereCollider.center, sphereCollider.radius + fadeDistance);

                    DrawSphereFadeLine(Vector3.left);
                    DrawSphereFadeLine(Vector3.right);
                    DrawSphereFadeLine(Vector3.up);
                    DrawSphereFadeLine(Vector3.down);
                    DrawSphereFadeLine(Vector3.forward);
                    DrawSphereFadeLine(Vector3.back);
                }

                void DrawSphereFadeLine(Vector3 direction)
                {
                    Gizmos.DrawLine(sphereCollider.center + direction * sphereCollider.radius, direction * (sphereCollider.radius + fadeDistance));
                }
            }
            else if (shape is CapsuleCollider capsuleCollider)
            {
                Gizmos.DrawWireSphere(capsuleCollider.center + Vector3.up * (capsuleCollider.height / 2 - capsuleCollider.radius), capsuleCollider.radius);
                Gizmos.DrawWireSphere(capsuleCollider.center + Vector3.down * (capsuleCollider.height / 2 - capsuleCollider.radius), capsuleCollider.radius);

                DrawCapsuleLine(Vector3.forward);
                DrawCapsuleLine(Vector3.back);
                DrawCapsuleLine(Vector3.right);
                DrawCapsuleLine(Vector3.left);

                Gizmos.color *= new Color(1, 1, 1, 0.5f);
                Gizmos.DrawSphere(capsuleCollider.center + Vector3.up * (capsuleCollider.height / 2 - capsuleCollider.radius), capsuleCollider.radius);
                Gizmos.DrawSphere(capsuleCollider.center + Vector3.down * (capsuleCollider.height / 2 - capsuleCollider.radius), capsuleCollider.radius);

                if (drawFade)
                {
                    Gizmos.DrawWireSphere(capsuleCollider.center + Vector3.up * (capsuleCollider.height / 2 - capsuleCollider.radius), capsuleCollider.radius + fadeDistance);
                    Gizmos.DrawWireSphere(capsuleCollider.center + Vector3.down * (capsuleCollider.height / 2 - capsuleCollider.radius), capsuleCollider.radius + fadeDistance);
                }

                void DrawCapsuleLine(Vector3 side)
                {
                    Gizmos.DrawLine(capsuleCollider.center + Vector3.up * (capsuleCollider.height / 2 - capsuleCollider.radius) + side * capsuleCollider.radius,
                    capsuleCollider.center + Vector3.down * (capsuleCollider.height / 2 - capsuleCollider.radius) + side * capsuleCollider.radius);
                }
            }
            else if (shape is MeshCollider meshCollider)
            {
                Gizmos.DrawWireMesh(meshCollider.sharedMesh);

                Gizmos.color *= new Color(1, 1, 1, 0.5f);
                Gizmos.DrawMesh(meshCollider.sharedMesh);
            }

            Gizmos.matrix = originalMatrix;
        }
    }
}
