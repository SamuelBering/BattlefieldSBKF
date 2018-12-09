namespace BattlefieldSBKF.Models
{
    public class Response
    {

        public Responses Resp { get; set; }
        public string Parameter { get; set; }

        public Response(Responses resp, string parameter)
        {
            Resp = resp;
            Parameter = parameter;
        }

    }
}