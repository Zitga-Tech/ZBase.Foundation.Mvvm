﻿






///*************************************************///
///                                                 ///
/// This file is auto-generated by T4 Text Template ///
///           DO NOT manually modify it!            ///
///                                                 ///
///*************************************************///

using System;
using ZBase.Foundation.Mvvm.Unions;

namespace ZBase.Foundation.Mvvm.ViewBinding.Adapters
{

    [Serializable]
    [Label("Bool ⇒ String", "Default")]
    [Adapter(fromType: typeof(bool), toType: typeof(string), order: 0)]
    public sealed class BoolToStringAdapter : IAdapter
    {
        public Union Convert(in Union union)
        {
            if (union.TryGetValue(out bool result))
            {
                return result.ToString();
            }

            return union;
        }
    }

    [Serializable]
    [Label("Byte ⇒ String", "Default")]
    [Adapter(fromType: typeof(byte), toType: typeof(string), order: 0)]
    public sealed class ByteToStringAdapter : IAdapter
    {
        public Union Convert(in Union union)
        {
            if (union.TryGetValue(out byte result))
            {
                return result.ToString();
            }

            return union;
        }
    }

    [Serializable]
    [Label("SByte ⇒ String", "Default")]
    [Adapter(fromType: typeof(sbyte), toType: typeof(string), order: 0)]
    public sealed class SByteToStringAdapter : IAdapter
    {
        public Union Convert(in Union union)
        {
            if (union.TryGetValue(out sbyte result))
            {
                return result.ToString();
            }

            return union;
        }
    }

    [Serializable]
    [Label("Char ⇒ String", "Default")]
    [Adapter(fromType: typeof(char), toType: typeof(string), order: 0)]
    public sealed class CharToStringAdapter : IAdapter
    {
        public Union Convert(in Union union)
        {
            if (union.TryGetValue(out char result))
            {
                return result.ToString();
            }

            return union;
        }
    }

    [Serializable]
    [Label("Double ⇒ String", "Default")]
    [Adapter(fromType: typeof(double), toType: typeof(string), order: 0)]
    public sealed class DoubleToStringAdapter : IAdapter
    {
        public Union Convert(in Union union)
        {
            if (union.TryGetValue(out double result))
            {
                return result.ToString();
            }

            return union;
        }
    }

    [Serializable]
    [Label("Float ⇒ String", "Default")]
    [Adapter(fromType: typeof(float), toType: typeof(string), order: 0)]
    public sealed class FloatToStringAdapter : IAdapter
    {
        public Union Convert(in Union union)
        {
            if (union.TryGetValue(out float result))
            {
                return result.ToString();
            }

            return union;
        }
    }

    [Serializable]
    [Label("Int ⇒ String", "Default")]
    [Adapter(fromType: typeof(int), toType: typeof(string), order: 0)]
    public sealed class IntToStringAdapter : IAdapter
    {
        public Union Convert(in Union union)
        {
            if (union.TryGetValue(out int result))
            {
                return result.ToString();
            }

            return union;
        }
    }

    [Serializable]
    [Label("UInt ⇒ String", "Default")]
    [Adapter(fromType: typeof(uint), toType: typeof(string), order: 0)]
    public sealed class UIntToStringAdapter : IAdapter
    {
        public Union Convert(in Union union)
        {
            if (union.TryGetValue(out uint result))
            {
                return result.ToString();
            }

            return union;
        }
    }

    [Serializable]
    [Label("Long ⇒ String", "Default")]
    [Adapter(fromType: typeof(long), toType: typeof(string), order: 0)]
    public sealed class LongToStringAdapter : IAdapter
    {
        public Union Convert(in Union union)
        {
            if (union.TryGetValue(out long result))
            {
                return result.ToString();
            }

            return union;
        }
    }

    [Serializable]
    [Label("ULong ⇒ String", "Default")]
    [Adapter(fromType: typeof(ulong), toType: typeof(string), order: 0)]
    public sealed class ULongToStringAdapter : IAdapter
    {
        public Union Convert(in Union union)
        {
            if (union.TryGetValue(out ulong result))
            {
                return result.ToString();
            }

            return union;
        }
    }

    [Serializable]
    [Label("Short ⇒ String", "Default")]
    [Adapter(fromType: typeof(short), toType: typeof(string), order: 0)]
    public sealed class ShortToStringAdapter : IAdapter
    {
        public Union Convert(in Union union)
        {
            if (union.TryGetValue(out short result))
            {
                return result.ToString();
            }

            return union;
        }
    }

    [Serializable]
    [Label("UShort ⇒ String", "Default")]
    [Adapter(fromType: typeof(ushort), toType: typeof(string), order: 0)]
    public sealed class UShortToStringAdapter : IAdapter
    {
        public Union Convert(in Union union)
        {
            if (union.TryGetValue(out ushort result))
            {
                return result.ToString();
            }

            return union;
        }
    }

}
