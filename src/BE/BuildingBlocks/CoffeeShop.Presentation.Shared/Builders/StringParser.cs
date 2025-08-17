namespace CoffeeShop.Presentation.Shared.Builders
{
    public class StringParser : IValueParser<string>
    {
        public string Parse(string value)
        {
            return value;
        }
    }
}
