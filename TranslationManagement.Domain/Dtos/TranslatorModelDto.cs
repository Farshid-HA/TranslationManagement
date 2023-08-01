using Domain.Enums;

namespace Domain.Dtos
{
    public class TranslatorModelDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HourlyRate { get; set; }
        public TranslatorStatusEnum Status { get; set; }
        public string CreditCardNumber { get; set; }
    }
}
