using UnityEngine;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    public static class Logger
    {
        [HideInCallstack]
        public static void WarnIfTargetListIsEmpty(UnityEngine.Object context)
        {
            Debug.LogWarning("The target list is empty.", context);
        }
    }
}