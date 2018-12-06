namespace BattlefieldSBKF.Models
{
    public class AnswerCode
    {

        public string Name { get; set; }
        public string Description { get; set; }

        public AnswerCode(string name, string description)
        {
            Name = name;
            Description = description;
        }

    }
}