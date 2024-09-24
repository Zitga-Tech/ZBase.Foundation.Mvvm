using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;
using ZBase.Foundation.Mvvm.Unity.ViewBinding;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace Mvvm.Samples
{
    [AddComponentMenu("MVVM/Binders/Random Scroll Bar Binder")]
    public partial class RandomScrollbarBinder : MonoBinder<Scrollbar>, IBinder
    {
        protected sealed override void OnAwake([NotNull] ref Scrollbar[] targets)
        {
            if (targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<Scrollbar>(out var target))
                {
                    targets = new Scrollbar[] { target };
                }
            }
        }

        [BindingProperty]
        [field: Label("Random Value")]
        [field: HideInInspector]
        private void SetRandomValue()
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].SetValueWithoutNotify(Random.Range(0f, 1f));
            }
        }
    }
}
