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
    }
}