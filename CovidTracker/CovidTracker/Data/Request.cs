using System;
namespace CovidTracker
{
    public class Request
    {
        public static readonly string REGISTER = "register";
        public static readonly string GET_RISK = "risk";

        public string id;
        public string action;

        public Request(string action)
        {
            this.action = action;
        }
    }
}
