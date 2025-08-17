namespace CoffeeShop.Presentation.Shared.Builders
{
    public class NumbericArrayParser : IValueArrayParser<double>
    {
        public double[] Parse(string value)
        {
            var filterArray = value.TrimStart('[').TrimEnd(']').Split(',');
            var values = filterArray.Select(x => double.Parse(x.ToString())).ToArray();
            return values;
        }
    }
}
