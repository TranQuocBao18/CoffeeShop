using CoffeeShop.Model.Dto.Shared.Filters;

namespace CoffeeShop.Presentation.Shared.Builders
{
    public class NotContainsOperationBuilder : BaseBuilder
    {
        protected override string OPERATION => "Not contains";

        public NotContainsOperationBuilder(IValueParser<string> stringParser) : base(stringParser)
        {
            supportOperations.Clear();
            supportOperations.Add(PageSearchType.TEXT, BuildText);
        }

        protected override Tuple<string, object[], string[]> BuildOperationSqlText(string fieldName, string value)
        {
            string sql = $"{fieldName}.CONTAINS(@notContainsValue) == false";
            return new Tuple<string, object[], string[]>(sql, new object[] { value }, new string[] { "@notContainsValue" });
        }
    }
}
