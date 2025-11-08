using System;

namespace SparseInject
{
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public class AutoLinkInstallerAttribute : Attribute
    {
        public Type InstallerType { get; }

        public AutoLinkInstallerAttribute(Type installerType)
        {
            InstallerType = installerType;
        }
    }
}