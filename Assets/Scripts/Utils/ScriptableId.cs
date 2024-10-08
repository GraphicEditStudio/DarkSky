using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Utils
{
    public class ScriptableId : ScriptableObject
    {
        [SerializeField]
        private string id;
        public string Id => id;
        public void OnValidate()
        {
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString();
            }
        }

        [ContextMenu("Regenerate Id")]
        public void RegenerateId()
        {
            id = Guid.NewGuid().ToString();
        }
    }
}