namespace SparseInject
{
    public struct RegistrationOptions
    {
        internal ContainerBuilder _builder;
        internal int _concreteIndex;

        public void MarkDisposable()
        {
            _builder.MarkConcreteDisposable(_concreteIndex);
        }
    }
}