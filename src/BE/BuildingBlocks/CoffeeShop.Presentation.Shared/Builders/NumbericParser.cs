namespace CoffeeShop.Presentation.Shared.Builders
{
    public class NumbericParser : IValueParser<double>
    {
        public double Parse(string value)
        {
            return double.Parse(value);
        }
    }
}
