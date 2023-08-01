using Domain.Enums;

namespace Domain.Dtos
{
    public class TranslationJobDto
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public JobStatusesEnum Status { get; set; }
        public string OriginalContent { get; set; }
        public string TranslatedContent { get; set; }
        public int? TranslatorId { get; set; }
        public double Price { get; set; }
    }
}
