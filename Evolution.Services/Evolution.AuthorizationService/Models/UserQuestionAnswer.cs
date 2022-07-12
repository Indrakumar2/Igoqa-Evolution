using System.Collections.Generic;

namespace Evolution.AuthorizationService.Models
{
    public class UserQuestionAnswer
    {
        public string Application { get; set; }

        public string UserLogonName { get; set; }

        public string UserEmail { get; set; }

        public IList<QuestionAnswer> QuestionAnswers { get; set; }
    }

    public class QuestionAnswer
    {
        public string Question { get; set; }

        public string Answer { get; set; }
    }
}
