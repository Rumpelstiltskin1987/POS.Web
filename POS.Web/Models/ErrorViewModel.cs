namespace POS.Web.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public string? Message { get; set; }

        public string? Source { get; set; }

        public string? InnerExceptionMessage { get; set; }

        public string? InnerExceptionSource { get; set; }
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public bool ShowInnerException => !string.IsNullOrEmpty(InnerExceptionMessage);
    }
}
