using System;
namespace CovidTracker
{
    public class Request
    {
        public static readonly string REGISTER = "register";
        public static readonly string GET_RISK = "get_risk";

        public string action;

        public Request(string action)
        {
            this.action = action;
        }
    }
}
