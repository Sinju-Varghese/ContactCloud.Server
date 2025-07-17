using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace ContactCloud.Common.Types;

public class Result<TData>
{
    public bool Success => ResultType == ResultTypes.Success;

    public string? Message { get; set; }

    public List<string> Errors { get; set; } = new List<string>();

    public TData? Data { get; set; } 

    [JsonIgnore]
    public ResultTypes ResultType { get; private set; }

    public Result() 
    { 
        ResultType = ResultTypes.Success; 
    }

    public Result(ResultTypes resultType)
    {
        ResultType = resultType;
    }

    public Result<TData> AddError(string error, ResultTypes type)
    {
        Errors.Add(error);
        ResultType = type;
        return this;
    }

    public IActionResult ToActionResult()
    {
        switch (ResultType)
        {
            case ResultTypes.Success:
            case ResultTypes.CompletedWithErrors:
                return new OkObjectResult(this);
            case ResultTypes.NotFound:
                return new NotFoundObjectResult(this);
            case ResultTypes.InvalidData:
                return new BadRequestObjectResult(this);
            case ResultTypes.PermissionDenied:
                return new ForbidResult();
            default:
                return new BadRequestObjectResult(this);
        }
    }
}
