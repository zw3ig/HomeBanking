﻿using HomeBanking.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace HomeBanking.Repositories
{
    public class AccountRepository: RepositoryBase<Account>, IAccountRepository
    {
        public AccountRepository(HomeBankingContext repositoryContext) : base(repositoryContext) { }
        public Account FindById(long id)
        {
            return FindByCondition(account => account.Id == id)
                .Include(account => account.Transactions)
                .FirstOrDefault();
        }

        public IEnumerable<Account> GetAllAccounts()
        {
            return FindAll()
                .Include(account => account.Transactions)
                .ToList();
        }

        public void Save(Account account)
        {
            Create(account);
            SaveChanges();
        }

        public IEnumerable<Account> GetAccountsByClient(long clientId)
        {
            return FindByCondition(account => account.ClientId == clientId)
                .Include(account => account.Transactions)
                .ToList();
        }

        public string GetLastAccountNumber()
        {
            return FindAll()
                .OrderByDescending(account => account.CreationDate)
                .Select(account => account.Number)
                .FirstOrDefault()
                .ToString();
            
        }
    }
}
