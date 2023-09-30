using CodeFlix.Catalog.Application.UseCases.Category.GetCategory;
using FluentValidation;


namespace CodeFlix.Catalog.UnitTest.Application.Category.GetCategory;
public class GetCategoryInputValidatior :
    AbstractValidator<GetCategoryInput>
{
    public GetCategoryInputValidatior()
    {
        RuleFor(RuleFor => RuleFor.Id)
            .NotEmpty();
    }

}
