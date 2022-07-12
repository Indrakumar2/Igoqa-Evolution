using Nest;
using System;

namespace Evolution.ElasticSearch
{
    public class EvolutionElasticSearchDocument
    {
        public Guid Id { get; set; }

        [Text(Name = "name")]
        public string Name { get; set; }

        [Text(Name = "description")]
        public string Description { get; set; }

        [Keyword(Name = "tag")]
        public string[] Tags { get; set; }

        public string DocumentType { get; set; }

        public string VisibleToTS { get; set; }

        public string VisibleToCustomer { get; set; }
        
        public DateTime? UploadedOn { get; set; }
    }
}
