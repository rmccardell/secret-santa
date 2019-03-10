using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using secret_santa.core.Contracts.Services;
using secret_santa.core.Contracts.Gateways;
using secret_santa.core.Entities;
using secret_santa.infrastructure.Services;

namespace secret_santa.infrastructure.Data.Repositories
{
    public class InMemoryExchangeMemberRepository : IExchangeMemberRepository
    {
        private static readonly List<Member> Members;
        private IExchangeListGenerator _exchangeListGenerator = new ExchangeListGenerator();

        static InMemoryExchangeMemberRepository()
        {
            Members = new List<Member>
            {
                new Member
                {
                    Name = "Sam Gold",
                    Id = Guid.NewGuid()
                },

                new Member
                {
                    Name = "Tracey White",
                    Id = Guid.NewGuid()
                }
            };

        }

        public List<Member> GetAllMembers()
        {
            return Members;
        }

        public Member FindMember(string id)
        {
            if (Guid.TryParse(id, out Guid guid))
            {
                var existingMember = Members.FirstOrDefault(m => m.Id == guid);
                return existingMember;
            }

            return null;

        }

        public Member AddMember(Member member)
        {
            try
            {
                member.Id = Guid.NewGuid();
                Members.Add(member);
                return member;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Member RemoveMember(Member member)
        {

            var existingMember = FindMember(member.Id.ToString());

            if (existingMember == null)
            {
                throw new EntityNotFoundException($"member with id {member.Id} not found");
            }


            Members.Remove(existingMember);

            return member;
        }

        public Member UpdateMember(Member member)
        {
            var existingMember = FindMember(member.Id.ToString());

            if (existingMember == null)
            {
                throw new EntityNotFoundException($"member with id {member.Id} not found");
            }

            existingMember.Name = member.Name;
            existingMember.SelectiveMatch = member.SelectiveMatch;

            return existingMember;
        }

        public void AddToNoExchangeList(Member member)
        {
            var existingMember = FindMember(member.Id.ToString());

            if (existingMember == null)
            {
                throw new EntityNotFoundException($"member with id {member.Id} not found");
            }

        }

        public void RemoveFromNoExchangeList(Member member)
        {
            var existingMember = FindMember(member.Id.ToString());

            if (existingMember == null)
            {
                throw new EntityNotFoundException($"member with id {member.Id} not found");
            }

        }

        public List<Match<Member>> GenerateMatchResults()
        {
            return _exchangeListGenerator.Generate(Members, null);
        }
    }
}
