namespace MailBC.UI.Infrastructure.Dependency
{
    public interface IDependencyResolver
    {
        T LocateDependency<T>(); 
    }
}