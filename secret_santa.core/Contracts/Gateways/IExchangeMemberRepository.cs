using System.Collections.Generic;
using secret_santa.core.Entities;

namespace secret_santa.core.Contracts.Gateways
{
    public interface IExchangeMemberRepository
    {
        List<Member> GetAllMembers();

        Member FindMember(string id);
        Member AddMember(Member member);
        Member RemoveMember(Member member);
        Member UpdateMember(Member member);

        List<Match<Member>> GenerateMatchResults(); 

    }

}