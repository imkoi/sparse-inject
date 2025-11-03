using System;
using System.Collections.Generic;

namespace SparseInject
{
    internal static class AutoLinkInstallersFactory
    {
        public static IEnumerable<IInstaller> Create()
        {
            var installers = new List<IInstaller>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var type = typeof(AutoLinkInstallerAttribute);

            foreach (var assembly in assemblies)
            {
                foreach (var assemblyAttribute in assembly.GetCustomAttributes(type, true))
                {
                    if (assemblyAttribute is AutoLinkInstallerAttribute linkInstallerAttribute)
                    {
                        var installer = Activator.CreateInstance(linkInstallerAttribute.InstallerType) as IInstaller;
                        
                        installers.Add(installer);
                    }
                }
            }
            
            return installers;
        }
    }
}