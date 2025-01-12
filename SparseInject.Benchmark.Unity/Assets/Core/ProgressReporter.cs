using System;

namespace SparseInject.BenchmarkFramework
{
    public class BenchmarkProgress : IProgress<float>
    {
        public event Action<float> Changed;
        
        private float _lastValue;
        
        public void Report(float value)
        {
            if (Math.Abs(_lastValue - value) > float.Epsilon)
            {
                _lastValue = value;
                Changed?.Invoke(_lastValue);
            }
        }
    }
}