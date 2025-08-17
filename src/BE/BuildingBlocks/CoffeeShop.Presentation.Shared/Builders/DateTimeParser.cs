using System.Globalization;
using CoffeeShop.Model.Dto.Shared.Constants;

namespace CoffeeShop.Presentation.Shared.Builders
{
    public class DateTimeParser : IValueParser<DateTime>
    {
        public DateTime Parse(string value)
        {
            var date = DateTime.ParseExact(value, JsonConstant.DefaultDateTimeFormat, CultureInfo.InvariantCulture);
            return date;
        }
    }
}
