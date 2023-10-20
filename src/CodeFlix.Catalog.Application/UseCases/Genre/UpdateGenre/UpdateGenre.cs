using CodeFlix.Catalog.Application.Interfaces;
using CodeFlix.Catalog.Application.UseCases.Genre.Common;
using CodeFlix.Catalog.Domain.Domain.Repository;
using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Genre.UpdateGenre;
public class UpdateGenre : IRequestHandler<UpdateGenreInput, GenreModelOutput>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateGenre(
        IGenreRepository genreRepository,
        IUnitOfWork unitOfWork,
        ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
        _genreRepository = genreRepository;
        _unitOfWork = unitOfWork;
    }


    public async Task<GenreModelOutput> Handle(
        UpdateGenreInput request,
        CancellationToken cancellationToken)
    {
        var genre = await _genreRepository
            .Get(request.Id, cancellationToken);

        genre.Update(request.Name);

        if (request.IsActive != genre.IsActive
            && request.IsActive is not null)
        {
            if ((bool)request.IsActive)
            {
                genre.Activate();
            }
            else
            {
                genre.Deactivate();
            }
        }
        await _genreRepository.Update(genre, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return GenreModelOutput.FromGenre(genre);

    }
}
