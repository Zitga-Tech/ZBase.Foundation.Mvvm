using System;
using ZBase.Foundation.Mvvm.Input;

namespace ZBase.Foundation.Mvvm.ViewBinding
{
    [Serializable]
    public class BindingCommand
    {
        /// <summary>
        /// The <see cref="ZBase.Foundation.Mvvm.Input.ICommand"/> whose container class is an <see cref="ZBase.Foundation.Mvvm.ComponentModel.IObservableObject"/>.
        /// </summary>
#if UNITY_5_3_OR_NEWER
        [field: UnityEngine.SerializeField]
#endif
        public string TargetCommandName { get; set; }
    }

    [Serializable]
    public sealed class BindingRelayCommand : BindingCommand
    {
        private IRelayCommand _command;

    }
}
