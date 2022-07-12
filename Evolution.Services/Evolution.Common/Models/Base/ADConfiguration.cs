namespace Evolution.Common.Models.Base
{
    public class ADConfiguration
    {
        public string LdapUrl { get; set; }
        public int LdapServerPort { get; set; }
        public string LdapDomainName { get; set; }
        public string LdapUser { get; set; }
        public string LdapPswd { get; set; }
        public string LdapSearchBase { get; set; }
        public string LdapSearchFilter { get; set; }
        public string LdapServiceAccountDn { get; set; }
        public bool EnableSecureSocketLayer { get; set; }
        public bool IsSandBoxEnvironment { get; set; }
    }
}
