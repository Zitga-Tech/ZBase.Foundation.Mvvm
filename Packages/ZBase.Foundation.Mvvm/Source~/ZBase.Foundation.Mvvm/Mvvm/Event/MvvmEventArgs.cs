using ZBase.Foundation.Mvvm.Unions;

namespace ZBase.Foundation.Mvvm.Event
{
    public readonly struct MvvmEventArgs
    {
        public readonly object Sender;
        public readonly Union Value;
        
        public MvvmEventArgs(object sender, in Union value)
        {
            Sender = sender;
            Value = value;
        }
    }
}
