
using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Genre.DeleteGenre;
public interface IDeleteGenre :
    IRequestHandler<DeleteGenreInput>
{
}
