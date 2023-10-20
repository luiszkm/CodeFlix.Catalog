
using CodeFlix.Catalog.Application.UseCases.Genre.Common;
using MediatR;

namespace CodeFlix.Catalog.Application.UseCases.Genre.UpdateGenre;
public interface IUpdateGenre :
    IRequestHandler<UpdateGenreInput, GenreModelOutput>
{

}
