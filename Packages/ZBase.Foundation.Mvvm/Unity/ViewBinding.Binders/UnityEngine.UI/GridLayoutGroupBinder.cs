#if UNITY_UGUI

using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/Grid Layout Group Binder")]
    public partial class GridLayoutGroupBinder : MonoBinder<GridLayoutGroup>
    {
        protected override void OnAwake([NotNull] ref GridLayoutGroup[] targets)
        {
            if (targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<GridLayoutGroup>(out var target))
                {
                    targets = new GridLayoutGroup[] { target };
                }
            }
        }

        [BindingProperty]
        [field: Label("Cell Size")]
        [field: HideInInspector]
        private void SetCellSize(Vector2 value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].cellSize = value;
            }
        }

        [BindingProperty]
        [field: Label("Cell Width")]
        [field: HideInInspector]
        private void SetCellWidth(float value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];
                target.cellSize = new Vector2(value, target.cellSize.y);
            }
        }

        [BindingProperty]
        [field: Label("Cell Height")]
        [field: HideInInspector]
        private void SetCellHeight(float value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];
                target.cellSize = new Vector2(target.cellSize.x, value);
            }
        }

        [BindingProperty]
        [field: Label("Spacing Size")]
        [field: HideInInspector]
        private void SetSpacingSize(Vector2 value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].spacing = value;
            }
        }

        [BindingProperty]
        [field: Label("Spacing Width")]
        [field: HideInInspector]
        private void SetSpacingWidth(float value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];
                target.spacing = new Vector2(value, target.spacing.y);
            }
        }

        [BindingProperty]
        [field: Label("Spacing Height")]
        [field: HideInInspector]
        private void SetSpacingHeight(float value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];
                target.spacing = new Vector2(target.spacing.x, value);
            }
        }

        [BindingProperty]
        [field: Label("Start Corner")]
        [field: HideInInspector]
        private void SetStartCorner(GridLayoutGroup.Corner value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].startCorner = value;
            }
        }

        [BindingProperty]
        [field: Label("Start Axis")]
        [field: HideInInspector]
        private void SetStartAxis(GridLayoutGroup.Axis value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].startAxis = value;
            }
        }
        
        [BindingProperty]
        [field: Label("Child Alignment")]
        [field: HideInInspector]
        private void SetChildAlignment(TextAnchor value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].childAlignment = value;
            }
        }

        [BindingProperty]
        [field: Label("Constraint")]
        [field: HideInInspector]
        private void SetConstraint(GridLayoutGroup.Constraint value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].constraint = value;
            }
        }

        [BindingProperty]
        [field: Label("Constraint Count")]
        [field: HideInInspector]
        private void SetConstraintCount(int value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].constraintCount = value;
            }
        }
    }
}

#endif
