
using CodeFlix.Catalog.Application.UseCases.Category.ListCategories;

namespace CodeFlix.Catalog.UnitTest.Application.Category.ListCategory;
public class ListCategoriesTestDataGenerator
{
    public static IEnumerable<object[]> GetInputsWothoutAllParameter(int time = 12)
    {
        var fixture = new ListCategoryTestFixture();
        var inputExample = fixture.GetExampleInput();

        for (int i = 0; i < time; i++)
        {

            switch (i % 6)
            {
                case 0:
                    yield return new object[] { new ListCategoriesInput() };
                    break;
                case 1:
                    yield return new object[] { new ListCategoriesInput(
                        inputExample.Page) };
                    break;
                case 2:
                    yield return new object[] { new ListCategoriesInput(
                        inputExample.Page,
                        inputExample.PerPage) };
                    break;
                case 3:
                    yield return new object[] { new ListCategoriesInput(
                        inputExample.Page,
                        inputExample.PerPage,
                        inputExample.Search) };
                    break;
                case 4:
                    yield return new object[] { new ListCategoriesInput(
                        inputExample.Page,
                        inputExample.PerPage,
                        inputExample.Search,
                        inputExample.Sort
                        )};
                    break;
                case 5:
                    yield return new object[] { new ListCategoriesInput(
                        inputExample.Page,
                        inputExample.PerPage,
                        inputExample.Search,
                        inputExample.Sort,
                        inputExample.Dir
                    )};
                    break;

                default:
                    yield return new object[] { new ListCategoriesInput() };
                    break;
            }
        }

    }
}
