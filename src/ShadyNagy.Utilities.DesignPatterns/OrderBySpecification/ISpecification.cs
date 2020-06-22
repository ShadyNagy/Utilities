namespace ShadyNagy.Utilities.DesignPatterns.OrderBySpecification
{
    public interface ISpecification<TEntity>
        where TEntity : class
    {
        Configuration<TEntity> Internal { get; }
    }
}
