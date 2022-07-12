using Evolution.Common.Models.Notes;
using System;

namespace Evolution.ResourceSearch.Domain.Models.ResourceSearch
{
    public class ResourceSearchNote: Note
    {
        //public int Id { get; set; }
        public int ResourceSearchId { get; set; }
        public int? AssignmentId { get; set; }
    }
}
