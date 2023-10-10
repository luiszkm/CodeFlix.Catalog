namespace CodeFlix.Catalog.Application.Interfaces;
public interface IUnitOfWork
{
    public Task Commit(CancellationToken cancellationToken);

    public Task Rollback(CancellationToken cancellationToken);
}
