﻿namespace DevIO.NerdStore.Core.Communication;

public class ResponseResult
{
    public string? Title { get; set; } = string.Empty;
    public int Status { get; set; }
    public ResponseErrorMessages Errors { get; set; } = new();
}

public class ResponseErrorMessages
{
    public List<string> Mensagens { get; set; } = [];
}