namespace CoffeeShop.Presentation.Shared.Builders
{
    public class BoolParser : IValueParser<bool>
    {
        public bool Parse(string value)
        {
            return bool.Parse(value);
        }
    }
}
