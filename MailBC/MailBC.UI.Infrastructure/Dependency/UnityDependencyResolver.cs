using Microsoft.Practices.Unity;

namespace MailBC.UI.Infrastructure.Dependency
{
    public class UnityDependencyResolver : IDependencyResolver
    {
        private readonly IUnityContainer _container;

        public UnityDependencyResolver(UnityContainer container)
        {
            _container = container;
        }

        public T LocateDependency<T>()
        {
            return _container.Resolve<T>();
        }

        public IUnityContainer RegisterType<T>(string name)
        {
            return _container.RegisterType<T>(name);
        }

        public IUnityContainer RegisterInstance<T>(T instance)
        {
            return _container.RegisterInstance<T>(instance);
        }
    }
}