namespace PaymentService.DTOs;

public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public static ApiResponse<T> Ok(T data, string message = "Get data successfully.")
    {
        return new ApiResponse<T> { Success = true, Message = message, Data = data };
    }

    public static ApiResponse<T> Created(T data, string message = "Created successfully.")
    {
        return new ApiResponse<T> { Success = true, Message = message, Data = data };
    }

    public static ApiResponse<T> Error(string message, int statusCode = 400)
    {
        return new ApiResponse<T> { Success = false, Message = message, Data = default };
    }

    public static ApiResponse<T> NotFound(string message = "Resource not found.")
    {
        return new ApiResponse<T> { Success = false, Message = message, Data = default };
    }

    public static ApiResponse<T> Conflict(string message)
    {
        return new ApiResponse<T> { Success = false, Message = message, Data = default };
    }
}

public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; }

    public static ApiResponse Ok(string message = "Success.")
    {
        return new ApiResponse { Success = true, Message = message };
    }

    public static ApiResponse Error(string message)
    {
        return new ApiResponse { Success = false, Message = message };
    }
}
