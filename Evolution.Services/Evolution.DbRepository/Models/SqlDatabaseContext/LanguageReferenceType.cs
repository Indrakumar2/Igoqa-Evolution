using System;
using System.Collections.Generic;

namespace Evolution.DbRepository.Models.SqlDatabaseContext
{
    public partial class LanguageReferenceType
    {
        public int Id { get; set; }
        public int ReferenceTypeId { get; set; }
        public int LanguageId { get; set; }
        public string Text { get; set; }
        public byte? UpdateCount { get; set; }
        public DateTime? LastModification { get; set; }

        public virtual Data Language { get; set; }
        public virtual Data ReferenceType { get; set; }
    }
}
