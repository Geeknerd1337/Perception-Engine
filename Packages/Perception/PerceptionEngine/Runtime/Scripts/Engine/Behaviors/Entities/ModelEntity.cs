using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Perception.Engine
{
    public class ModelEntity : Entity
    {
        /// <summary>
        /// The game object holding your model. For whatever reason an entity may have multiple renderers so this may be necessary
        /// for getting the correct one.
        /// </summary>
        public string ModelHolder = string.Empty;

        /// <summary>
        /// A reference to the renderer for this model.
        /// </summary>
        public Renderer MyRenderer { get; set; }

        private List<ModelAttachment> _attachments = new List<ModelAttachment>();
        public override void Start()
        {
            base.Start();

            _attachments.AddRange(GetComponentsInChildren<ModelAttachment>());
        }

        public override void Awake()
        {
            base.Awake();
            GetRenderer();
        }

        public ModelAttachment GetAttachment(string name)
        {
            return _attachments.FirstOrDefault(a => a.AttachmentName == name);
        }

        /// <summary>
		/// Attempts to get the renderer for our model entity
		/// </summary>
		private void GetRenderer()
        {
            //First, check to see if we can find a holder
            if (transform.Find(ModelHolder).gameObject is GameObject hold)
            {
                MyRenderer = hold.GetComponentInChildren<Renderer>();
            }
            else
            {
                //Otherwise, first check to see if this object has a renderer component
                if (GetComponentInChildren<Renderer>() is Renderer r)
                {
                    MyRenderer = r;
                }
                else
                {
                    Debug.LogWarning("Model Entity Contains No Renderer, did you foget to add one or could you be missing a reference?");
                }
            }
        }
    }
}
