using CodeFlix.Catalog.Application.Exceptions;
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
        if (request.CategoriesIds is not null)
        {
            genre.RemoveAllCategories();
            if (request.CategoriesIds.Count > 0)
            {
                await ValidateCategoriesIds(request, cancellationToken);
                request.CategoriesIds?.ForEach(categoryId =>
                    genre.AddCategory(categoryId));
            }
        }


        await _genreRepository.Update(genre, cancellationToken);
        await _unitOfWork.Commit(cancellationToken);

        return GenreModelOutput.FromGenre(genre);
    }

    private async Task ValidateCategoriesIds(
        UpdateGenreInput request,
        CancellationToken cancellationToken
    )
    {
        var IdsInPersistence = await _categoryRepository
            .GetIdsListByIds(
                request.CategoriesIds!,
                cancellationToken
            );
        if (IdsInPersistence.Count < request.CategoriesIds!.Count)
        {
            var notFoundIds = request.CategoriesIds
                .FindAll(x => !IdsInPersistence.Contains(x));
            var notFoundIdsAsString = string.Join(",", notFoundIds);
            throw new RelatedAggregateException(
                $"Related category id (or ids) not found: {notFoundIdsAsString}"
            );
        }
    }
}
