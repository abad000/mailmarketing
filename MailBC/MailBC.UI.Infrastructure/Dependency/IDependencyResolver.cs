using Microsoft.Practices.Unity;

namespace MailBC.UI.Infrastructure.Dependency
{
    public interface IDependencyResolver
    {
        T LocateDependency<T>();
        IUnityContainer RegisterType<T>(string name);
        IUnityContainer RegisterInstance<T>(T instance);
    }
}