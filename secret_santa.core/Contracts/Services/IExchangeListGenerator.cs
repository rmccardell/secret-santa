using System;
using System.Collections.Generic;
using secret_santa.core.Entities;

namespace secret_santa.core.Contracts.Services
{
   public interface IExchangeListGenerator
   {
       List<Match<Member>> Generate(List<Member> participants, int? maxAttempts);
   }
}