﻿using System.ComponentModel;

namespace API.Errors
{
    // I made this file to have a standard structure for every responses from my API
    [DisplayName("Api Response")]
    public class ApiResponse
    {

        public ApiResponse(int statusCode, string? message = null, object? data = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
            Data = data;
        }

        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }

        private string? GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                100 => "Continue",
                101 => "Switching Protocols",
                103 =>" Early Hints",
                200 => "OK",
                201 => "Created",
                202 => "Accepted",
                203 => "Non - Authoritative Information",
                204 => "No Content",
                205 => "Reset Content",
                206 => "Partial Content",
                300 => "Multiple Choices",
                301 => "Moved Permanently",
                302 => "Found",
                303 => "See Other",
                304 => "Not Modified",
                307 => "Temporary Redirect",
                308 => "Permanent Redirect",
                400 => "You have made a bad request.",
                401 => "You are not authorized.",
                402 => "Payment Required",
                403 => "You have not permission.",
                404 => "Not Found",
                405 => "Method Not Allowed",
                406 => "Not Acceptable",
                407 => "Proxy Authentication Required",
                408 => "Request Timeout",
                409 => "Conflict",
                410 => "Gone",
                411 => "Length Required",
                412 => "Precondition Failed",
                413 => "Payload Too Large",
                414 => "URI Too Long",
                415 => "Unsupported Media Type",
                416 => "Range Not Satisfiable",
                417 => "Expectation Failed",
                418 => "I'm a teapot",
                422 => "Unprocessable Entity",
                423 => "Locked",
                424 => "Failed Dependency",
                425 => "Too Early",
                426 => "Upgrade Required",
                428 => "Precondition Required",
                429 => "Too Many Requests",
                431 => "Request Header Fields Too Large",
                444 => "No Response",
                451 => "Unavailable For Legal Reasons",
                500 => "Internal Server Error!",
                501 => "Not Implemented",
                502 => "Bad Gateway",
                503 => "Service Unavailable",
                504 => "Gateway Timeout",
                505 => "HTTP Version Not Supported",
                506 => "Variant Also Negotiates",
                507 => "Insufficient Storage",
                508 => "Loop Detected",
                510 => "Not Extended",
                511 => "Network Authentication Required",
                _ => null
            };
        }
    }
}
