using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class TransformBinderEditor
    {
        [MenuItem("CONTEXT/Transform/Binder")]
        static void BindTransform(MenuCommand command)
        {
            if (command.context is Transform target)
            {
                Setup(target);
            }
        }

        public static MonoBehaviour Setup(Transform target)
        {
            var comp = Undo.AddComponent<TransformBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}
