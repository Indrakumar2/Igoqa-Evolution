using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class UserQuestionAnswer
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }

        public virtual User User { get; set; }
    }
}
