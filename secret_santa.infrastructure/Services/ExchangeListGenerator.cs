using System.Collections.Generic;
using System.Linq;
using secret_santa.core.Contracts.Services;
using secret_santa.core.Entities;
using secret_santa.infrastructure.Extensions;

namespace secret_santa.infrastructure.Services
{
    /// <summary>
    /// Responsible for match 
    /// </summary>
    public class ExchangeListGenerator : IExchangeListGenerator
    {

        public List<Match<Member>> Generate(List<Member> participants, int? maxAttempts)
        {
            int attemps = maxAttempts ?? 3;
            var matches = GenerateMatches(participants, attemps);

            return matches;
        }


        private static List<Match<Member>> GenerateMatches(List<Member> participants, int maxAttempts)
        {
            int attempts = maxAttempts;
            List<Member> availableMatches = new List<Member>(participants);
            List<Match<Member>> matches = new List<Match<Member>>();

            //randomly sort participant list
            participants.ShuffleList();

            foreach (var participant in participants)
            {
                //randomly pick a person from available matches simulating pulling
                //random name from fish bowl or hat
                var randomMember = availableMatches.GetRandom();

                if (randomMember == null)
                {
                    //this should technically never happen but we'll check for it anyway
                    var noMatchFound = NoMatchFound(participant);
                    matches.Add(noMatchFound);
                    continue;
                }

                //participants can't choose themselves
                if (randomMember.Id == participant.Id)
                {

                    if (availableMatches.Count == 0 || availableMatches.Count == 1)
                    {
                        var noMatchFound = NoMatchFound(participant);
                        matches.Add(noMatchFound);
                        continue;
                    }

                    if (availableMatches.Count == 2)
                    {
                        randomMember = availableMatches.FirstOrDefault(m => m.Id != participant.Id);
                    }
                    else
                    {

                        while (randomMember.Id == participant.Id)
                        {
                            randomMember = availableMatches.GetRandom();
                        }
                    }
                }

                //if participant is apart of the no match list
                //the matched member should not be apart of the no match list

                if (participant.SelectiveMatch && randomMember.SelectiveMatch)
                {
                    // we need to see if there are any available members 
                    // that are not to be selectively matched

                    var safeMatches = availableMatches
                    .Where(m => m.SelectiveMatch == false && m.Id != participant.Id).ToList();

                    if (safeMatches.Count == 0)
                    {
                        // if we get here there is a problem because there are no
                        // none selective match members available for the particpant
                        // to be matched with
                        var noMatchFound = NoMatchFound(participant);
                        matches.Add(noMatchFound);
                        continue;
                    }

                    randomMember = safeMatches.GetRandom();

                }

                var match = new Match<Member>(participant, randomMember);

                //remove the matched member from the list of available matches
                availableMatches.Remove(randomMember);

                matches.Add(match);

            }

            if (matches.Any(m => !string.IsNullOrEmpty(m.ErrorMessage)))
            {
                if (attempts > 0)
                {
                    attempts -= 1;

                    matches = GenerateMatches(participants, attempts);
                }
            }

            return matches;
        }

        private static Match<Member> NoMatchFound(Member participant)
        {
            var noMatchFound = new Match<Member>(participant, null)
            {
                ErrorMessage = "could not find a suitable match for participant"
            };
            return noMatchFound;
        }
    }
}