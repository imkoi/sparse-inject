using System;
using System.Reflection;

namespace SparseInject
{
    public static class ReflectionUtility
    {
        public static (ConstructorInfo info, ParameterInfo[] parameters) GetInjectableConstructor(Type type)
        {
            var constructors = type.GetConstructors(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
            var constructorParameters = default(ParameterInfo[]);
            var maxParametersCount = -1;

            for (var i = 0; i < constructors.Length; i++)
            {
                var suspectConstructor = constructors[i];
                                
                if (suspectConstructor.IsPublic || suspectConstructor.IsAssembly)
                {
                    var suspectConstructorParameters = suspectConstructor.GetParameters();

                    if (suspectConstructorParameters.Length > maxParametersCount)
                    {
                        constructorParameters = suspectConstructorParameters;
                        maxParametersCount = suspectConstructorParameters.Length;
                                    
                        constructors[0] = suspectConstructor;
                    }
                }
            }
                        
            if (maxParametersCount < 0)
            {
                throw new SparseInjectException($"Could not find public or internal constructor for type '{type}'");
            }
                        
            return (constructors[0], constructorParameters);
        }
    }
}