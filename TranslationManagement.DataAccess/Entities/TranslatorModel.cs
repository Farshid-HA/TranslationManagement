using Domain.Enums;

namespace DataAccess.Entities
{
    public class TranslatorModel:EntityBase<int>
    {
        public string Name { get; set; }
        public string HourlyRate { get; set; }
        public TranslatorStatusEnum Status { get; set; }
        public string CreditCardNumber { get; set; }
    }
}
