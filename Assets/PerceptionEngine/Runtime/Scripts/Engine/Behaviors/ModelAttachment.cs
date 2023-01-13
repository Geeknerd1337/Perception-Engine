using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Perception.Engine
{
    public class ModelAttachment : MonoBehaviour
    {
        public string AttachmentName = "Attachment";

        void OnValidate()
        {
            gameObject.name = AttachmentName;
        }
#if UNITY_EDITOR
        [MenuItem("GameObject/Netscape/Model Attachment")]
#endif
        public static void CreateAttachment()
        {
            GameObject attachment = new GameObject();
            attachment.name = "Attachment";
            attachment.transform.localPosition = Vector3.zero;
            attachment.transform.localRotation = Quaternion.identity;
            attachment.transform.localScale = Vector3.one;
            attachment.AddComponent<ModelAttachment>();
        }
    }
}
