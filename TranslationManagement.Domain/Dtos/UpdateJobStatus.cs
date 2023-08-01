using Domain.Enums;

namespace TranslationManagement.Domain.Dtos
{
    public class UpdateJobStatus
    {
        public int translatorId { get; set; }
        public int jobId { get; set; }
        public JobStatusesEnum newStatus { get; set; } = 0;
    }
}
