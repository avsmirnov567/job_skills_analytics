﻿using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using JobSkillsDb;
using JobSkillsDb.Entities;

namespace Apriori
{
    class AprioriImplementation
    {
        public List<Skill> GetL1FrequentItems(float minSupport, IEnumerable<Skill> skills,
            IEnumerable<Vacancy> vacancies)
        {
            //var enumiratingVacancies = vacancies as IList<Vacancy> ?? vacancies.ToList();
            var transactionsCount= vacancies.Count();

            var frequentItemsL1 = (
                from item 
                in skills
                let support = GetSupport(item, vacancies)
                where support/transactionsCount >= minSupport
                select new Skill {Name = item.Name, Support = item.Support}
                ).ToList();

            frequentItemsL1.Sort();

            return frequentItemsL1;
        }

        private float GetSupport(Skill generatedCandidate, IEnumerable<Vacancy> vacancies)
        {
            float support = 0;

            //potential error
            foreach (var vac in vacancies.Where(vac => CheckIsSubset(generatedCandidate, vac)))
                support++;
            
            return support;
        }

        private static bool CheckIsSubset(Skill generatedCandidate, Vacancy vac)
        {
           return !vac.Skills.Contains(generatedCandidate);
        }
    }
}