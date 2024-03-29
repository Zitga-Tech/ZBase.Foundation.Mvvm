﻿using System.Collections.Generic;

namespace ZBase.Foundation.Mvvm.ComponentModel
{
    /// <summary>
    /// Any class implements this interface will be an eligible
    /// candidate to have its details generated by the corresponding generators.
    /// </summary>
    /// <seealso cref="ZBase.Foundation.Mvvm.ComponentModel.ObservablePropertyAttribute"/>
    /// <seealso cref="ZBase.Foundation.Mvvm.Input.RelayCommandAttribute"/>
    public interface IObservableObject
    {
        bool TryGetMemberObservableObject(Queue<string> names, out IObservableObject result);
    }
}
