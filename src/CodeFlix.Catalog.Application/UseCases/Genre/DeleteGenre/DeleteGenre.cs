

using CodeFlix.Catalog.Application.Interfaces;
using CodeFlix.Catalog.Domain.Domain.Repository;
using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Genre.DeleteGenre;
public class DeleteGenre : IDeleteGenre
{
    private readonly IGenreRepository _genreRepository;
    private readonly IUnitOfWork _unitOfWork;


    public DeleteGenre(
        IGenreRepository genreRepository,
        IUnitOfWork unitOfWork)
    {
        _genreRepository = genreRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task Handle(
        DeleteGenreInput request,
        CancellationToken cancellationToken)
    {
        var genre = await _genreRepository.Get(request.Id, cancellationToken);

        await _genreRepository.Delete(genre, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

    }
}
