using CoffeeShop.Model.Dto.Shared.Filters;

namespace CoffeeShop.Presentation.Shared.Builders
{
    public class ContainsOperationBuilder : BaseBuilder
    {
        protected override string OPERATION => "Contains";

        public ContainsOperationBuilder(IValueParser<string> stringParser) : base(stringParser)
        {
            supportOperations.Clear();
            supportOperations.Add(PageSearchType.TEXT, BuildText);
        }

        protected override Tuple<string, object[], string[]> BuildOperationSqlText(string fieldName, string value)
        {
            string sql = $"{fieldName}.CONTAINS(@containValue)";
            return new Tuple<string, object[], string[]>(sql, new object[] { value }, new string[] { "@containValue" });
        }
    }
}
