﻿using HomeBanking.Models;
using System.Collections.Generic;
using System.Linq;

namespace HomeBanking.Repositories
{
    public class LoanRepository : RepositoryBase<Loan>, ILoanRepository
    {
        public LoanRepository(HomeBankingContext repositoryContext) : base(repositoryContext) { }

        public Loan FindById(long id)
        {
            return FindByCondition(x => x.Id == id)
                .FirstOrDefault();
        }

        public IEnumerable<Loan> GetAll()
        {
            return FindAll()
                .ToList();
        }
    }
}
