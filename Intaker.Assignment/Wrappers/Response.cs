namespace Intaker.Assignment.Wrappers;

public class Response<T>
{
    public Response(T data)
    {
        Data = data;
        Succeeded = true;
    }

    public Response(List<string> errors)
    {
        Errors = errors;
        Succeeded = false;
    }


    public bool Succeeded { get; set; }
    public List<string>? Errors { get; set; }
    public T? Data { get; set; }
}
