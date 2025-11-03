using System;

namespace SparseInject
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class AutoLinkInstallerAttribute : Attribute
    {
        public Type InstallerType { get; }

        public AutoLinkInstallerAttribute(Type installerType)
        {
            InstallerType = installerType;
        }
    }
}