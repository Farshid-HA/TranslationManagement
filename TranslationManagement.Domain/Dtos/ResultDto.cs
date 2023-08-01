using TranslationManagement.Domain.Enums;

namespace TranslationManagement.Domain.Dtos
{
    public class ResultDto<T>
    {
        public T Data { get; set; }
        public MessageCodes MessageCode { get; set; }

        public ResultDto()
        {
        }

        public ResultDto(T data)
        {
            Data = data;
            MessageCode = MessageCodes.Success;
        }
        public ResultDto(T data, MessageCodes messageCode)
        {
            Data = data;
            MessageCode = messageCode;
        }
    }

    public class ResultDto
    {
        public MessageCodes MessageCode { get; set; }
        public ResultDto()
        {
        }

        public ResultDto(MessageCodes messageCode)
        {
            MessageCode = messageCode;
        }

    }
}