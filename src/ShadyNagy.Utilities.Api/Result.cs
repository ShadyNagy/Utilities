using System;

namespace ShadyNagy.Utilities.Api
{


    public class Result<TData, TLookupData>
    {

        public enum MessageTypeEnum
        {
            Alert, 
            Error, 
            Info, 
            Warn, 
            Success
        }

        public bool IsRefreshToken { get; set; }

        public bool IsSuccess { get; set; }

        public int? ErrorCode { get; set; }

        public string Message { get; set; }

        public string MessageType { get; set; }

        public TData Data { get; set; }

        public TLookupData LookupData { get; set; }


        public Result(bool isRefreshToken, bool isSuccess, int errorCode, string message, string messageType, TData data)
        {
            IsRefreshToken = isRefreshToken;
            IsSuccess = isSuccess;
            ErrorCode = errorCode;
            Message = message;
            MessageType = messageType;
            Data = data;
        }


        public Result(bool isRefreshToken, bool isSuccess, int errorCode, string message, string messageType, TData data, TLookupData lookupData)
        {
            IsRefreshToken = isRefreshToken;
            IsSuccess = isSuccess;
            ErrorCode = errorCode;
            Message = message;
            MessageType = messageType;
            Data = data;
            LookupData = lookupData;
        }


        public Result(bool isRefreshToken, string message, TData data)
        {
            IsRefreshToken = isRefreshToken;
            IsSuccess = true;
            ErrorCode = null;
            Message = message;
            MessageType = MessageTypeEnum.Success.ToString();
            Data = data;
        }


        public Result(bool isRefreshToken, string message, TData data, TLookupData lookupData)
        {
            IsRefreshToken = isRefreshToken;
            IsSuccess = true;
            ErrorCode = null;
            Message = message;
            MessageType = MessageTypeEnum.Success.ToString();
            Data = data;
            LookupData = lookupData;
        }


        public Result(bool isRefreshToken, TData data)
        {
            IsRefreshToken = isRefreshToken;
            IsSuccess = true;
            ErrorCode = null;
            Message = null;
            MessageType = null;
            Data = data;
        }


        public Result(bool isRefreshToken, TData data, TLookupData lookupData)
        {
            IsRefreshToken = isRefreshToken;
            IsSuccess = true;
            ErrorCode = null;
            Message = null;
            MessageType = null;
            Data = data;
            LookupData = lookupData;
        }


        public Result()
        {
            IsSuccess = true;
            ErrorCode = null;
            Message = null;
            MessageType = MessageTypeEnum.Success.ToString();
            Data = default;
            LookupData = default;
        }


        public Result(bool isRefreshToken, string message)
        {
            IsRefreshToken = isRefreshToken;
            IsSuccess = true;
            ErrorCode = null;
            Message = message;
            MessageType = MessageTypeEnum.Success.ToString();
            Data = default;
            LookupData = default;
        }


        public Result<TData, TLookupData> ResultWithError(bool isRefreshToken, int errorCode, string errorMessage)
        {
            IsRefreshToken = isRefreshToken;
            IsSuccess = false;
            ErrorCode = errorCode;
            Message = errorMessage;
            MessageType = MessageTypeEnum.Error.ToString();
            Data = default;
            LookupData = default;

            return this;
        }


        public Result<TData, TLookupData> ResultWithDataAndLookup(bool isRefreshToken, TData data, TLookupData lookupData)
        {
            IsRefreshToken = isRefreshToken;
            Data = data;
            LookupData = lookupData;

            return this;
        }

        public Result<TData, TLookupData> ResultWithSuccess(bool isRefreshToken, string message)
        {
            IsRefreshToken = isRefreshToken;
            IsSuccess = true;
            Message = message;
            Data = default;
            LookupData = default;

            return this;
        }
    }
}
