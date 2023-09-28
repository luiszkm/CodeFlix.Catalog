
namespace CodeFlix.Catalog.UnitTest.Application.Category.UpdateCategory;

using CodeFlix.Catalog.UnitTest.Application.Category.CreateCategory;
using UseCase = CodeFlix.Catalog.Application.UseCases.Category.UpdateCategory;
public class UpdateCategoryDataGenerator
{
    public static IEnumerable<object[]> GetCategoriesToUpdate(int times = 10)
    {
        var fixture = new UpdateCategoryTestFixture();
        for (int i = 0; i < times; i++)
        {
            var exampleCategory = fixture.GetValidCategory();

            var input = fixture.GetValidInput(exampleCategory.Id);

            yield return new object[] { exampleCategory, input };
        }
    }

    public static IEnumerable<object[]> GetInvalidInputs(int times = 12)
    {
        var fixture = new UpdateCategoryTestFixture();
        var invalidInputsList = new List<object[]>();
        var totalInvalidCases = 3;

        for (int index = 0; index < times; index++)
        {
            switch (index % totalInvalidCases)
            {
                case 0:
                    invalidInputsList.Add(new object[] {
                        fixture.GetInvalidInputShortName(),
                        "Name should be at 3 characters long"
                    });
                    break;
                case 1:
                    invalidInputsList.Add(new object[] {
                        fixture.GetInvalidInputTooLongName(),
                        "Name should be less or equal 255 characters long"
                    });
                    break;
                case 2:
                    invalidInputsList.Add(new object[] {
                        fixture.GetInvalidInputTooLongDescription(),
                        "Description should be less or equal 10000 characters long"
                    });
                    break;
                default:
                    break;
            }
        }

        return invalidInputsList;
    }


}
