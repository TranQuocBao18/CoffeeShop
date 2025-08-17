namespace CoffeeShop.Presentation.Shared.Builders
{
    public class EqualsOperationBuilder : BaseBuilder
    {
        protected override string OPERATION => "Equals";

        public EqualsOperationBuilder(IValueParser<string> stringParser,
                                IValueParser<double> numbericParser,
                                IValueParser<bool> boolParser,
                                IValueParser<Guid> guidParser,
                                IValueParser<DateTime> dateParser) : base(stringParser, numbericParser, boolParser, guidParser, dateParser)
        {
        }
    }
}
