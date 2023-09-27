
namespace CodeFlix.Catalog.UnitTest.Application.Category.UpdateCategory;
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


}
