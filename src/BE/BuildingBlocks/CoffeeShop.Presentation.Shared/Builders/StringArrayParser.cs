namespace CoffeeShop.Presentation.Shared.Builders
{
    public class StringArrayParser : IValueArrayParser<string>
    {
        public string[] Parse(string value)
        {
            return value.TrimStart('[').TrimEnd(']').Split(',');
        }
    }
}
