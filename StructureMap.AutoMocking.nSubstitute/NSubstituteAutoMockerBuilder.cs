using System;
using System.Linq;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace StructureMap.AutoMocking.NSubstitute
{
    public static class NSubstituteAutoMockerBuilder
    {
        static readonly string AutoMockerCode =
            @"public class NSubstituteAutoMocker<T> : AutoMocker<T> where T : class
                          { 
                            public NSubstituteAutoMocker()
                                {
                                    _serviceLocator = new NSubstituteServiceLocator();
                                    _container = new AutoMockedContainer(_serviceLocator);
                                }
                          }";

        static readonly string ServiceLocatorCode =
            @"public class NSubstituteServiceLocator : ServiceLocator
                            {
                                private readonly SubstituteFactory _substituteFactory = new SubstituteFactory();
                                public T Service<T>() where T : class
                                {
                                    return (T)_substituteFactory.CreateMock(typeof(T));
                                }
                                
                                public object Service(Type serviceType)
                                {
                                    return _substituteFactory.CreateMock(serviceType);
                                }
                                
                                public T PartialMock<T>(params object[] args) where T : class
                                {
                                    return (T)_substituteFactory.CreateMock(typeof(T));
                                }
                            }";

        static readonly string NSubstituteFactoryCode =
            @"public class SubstituteFactory
                            {
                                private readonly Func<Type, object> _factory;
                                public SubstituteFactory()
                                {
                                    var assembly = Assembly.Load(""NSubstitute"");
                                    var type = assembly.GetType(""NSubstitute.Substitute"");
                                    if (type == null)
                                        throw new InvalidOperationException(""Can't find Substitute class in assembly @ "" + assembly.Location);

                                    var method = type.GetMethods().First(x => x.ContainsGenericParameters && x.GetGenericArguments().Length == 1);
                                    _factory = typeToMock => method.MakeGenericMethod(typeToMock).Invoke(null, new object[]{null});
                                } 
                                public object CreateMock(Type type)
                                {
                                    return _factory(type);
                                }
                            }";
        
        static readonly string AllCode = string.Format("using System; using System.Linq; using System.Reflection; namespace StructureMap.AutoMocking{{{0}{1}{2}}}", AutoMockerCode,
                                               ServiceLocatorCode, NSubstituteFactoryCode);

        static Type _assembledType;


        public static object Build<T>()
        {
            var type = _assembledType ?? (_assembledType = Compile(AllCode));

            var genericType = type.MakeGenericType(new[] { typeof(T) });

            return Activator.CreateInstance(genericType, new object[] {});
        }
        private static Type Compile(string code)
        {
            var param = new CompilerParameters();
            param.GenerateExecutable = false;
            param.GenerateInMemory = true;
            param.IncludeDebugInformation = false;
            param.ReferencedAssemblies.Add("StructureMap.AutoMocking.dll");
            param.ReferencedAssemblies.Add("StructureMap.dll");
            param.ReferencedAssemblies.Add("System.Xml.Linq.dll");
            param.ReferencedAssemblies.Add("System.Core.dll");

            var compiler = CSharpCodeProvider.CreateProvider("CSharp");

            var result = compiler.CompileAssemblyFromSource(param, code);

            if (result.Errors.Count != 0)
                throw new Exception("Could not compile");

            var assembly = result.CompiledAssembly;
            var assembledType =
                assembly.GetTypes().FirstOrDefault(
                    x => x.ContainsGenericParameters && x.Name.Contains("NSubstituteAutoMocker"));

            return assembledType;
        }
    }

}
