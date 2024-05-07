namespace JwtAuthorization.BL.Responses
{
    public class ErrorResponse<T>
    {
        public ErrorResponse() : this(new List<T>()) { }
        public ErrorResponse(T errorMessage) : this(new List<T>() { errorMessage }) { }
        public ErrorResponse(IEnumerable<T> errorMessages)
        {
            ErrorMessages = errorMessages;
        }

        public IEnumerable<T> ErrorMessages { get; set; }
    }
}