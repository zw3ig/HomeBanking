using HomeBanking.Models;
using System.Collections.Generic;

namespace HomeBanking.Repositories
{
    public interface ICardRepository
    {
        IEnumerable<Card> GetAllCards();
        void Save(Card card);
        Card FindById(long id);
        IEnumerable<Card> GetCardsByClient(long clientId);
    }
}
