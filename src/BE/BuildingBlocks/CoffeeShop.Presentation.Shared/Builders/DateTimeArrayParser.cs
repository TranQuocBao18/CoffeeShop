using System.Globalization;
using CoffeeShop.Model.Dto.Shared.Constants;

namespace CoffeeShop.Presentation.Shared.Builders
{
    public class DateTimeArrayParser : IValueArrayParser<DateTime>
    {
        public DateTime[] Parse(string value)
        {
            var filterArray = value.TrimStart('[').TrimEnd(']').Split(',');
            if (filterArray.Length != 2) throw new System.Exception($"Invalid input of type DateTime");
            DateTime fromDate = DateTime.ParseExact(filterArray[0].Trim(), JsonConstant.DefaultDateTimeFormat, CultureInfo.InvariantCulture);
            DateTime toDate = DateTime.ParseExact(filterArray[1].Trim(), JsonConstant.DefaultDateTimeFormat, CultureInfo.InvariantCulture);
            return new DateTime[] { fromDate, toDate };
        }
    }
}
