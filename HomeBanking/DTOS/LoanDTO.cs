namespace HomeBanking.DTOS
{
    public class LoanDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public double MaxAmount { get; set; }
        public string Payments { get; set; }
        //no se si va
        //public ICollection<ClientLoan> ClientLoans { get; set; }
    }
}
