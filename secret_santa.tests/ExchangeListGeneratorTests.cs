using System;
using System.Collections.Generic;
using System.Linq;
using secret_santa.core.Entities;
using secret_santa.infrastructure.Services;
using Xunit;

namespace secret_santa.tests
{
    public class ExchangeListGeneratorTests
    {
        [Fact]
        public void ShouldBeAbleToGenerateGiftExchangeListWithUniqueGitRecipientsForEachParticipant()
        {
            try
            {
                List<Member> members = new List<Member>
                {
                    new Member
                    {
                        Name = "Rick",
                        Id = new Guid("2cf0dfad-0f2f-4b54-913c-1146a706c1a6")
                    },

                    new Member
                    {
                        Name = "Reeba",
                        Id = new Guid("470cd67a-3383-4575-82bb-e067926afb0b")
                    },

                    new Member
                    {
                        Name = "Rhonda",
                        Id = new Guid("032f4c22-1d26-4a49-91f7-66bdb69588ed")
                    },
                    new Member
                    {
                        Name = "Ryan",
                        Id = new Guid("c0746e4b-38b5-4c57-b50a-2dc72611f712")
                    }
                };


                var exchangeListGen = new ExchangeListGenerator();

                var matches = exchangeListGen.Generate(members, null);
                
                //should be no errors
                var errors = matches.Where(m => !string.IsNullOrEmpty(m.ErrorMessage)).ToList();
                Assert.Empty(errors);


                foreach (var member in members)
                {
                    //each member should only appear once as participant and once as a recipient

                    var participants = matches.Where(m => m.Giver.Id == member.Id).Select(m=> new Member
                    {
                        Id = m.Giver.Id,
                        Name = m.Giver.Name
                    });
                    var recipients = matches.Where(m => m.Recipient.Id == member.Id).Select(m => new Member
                    {
                        Id = m.Recipient.Id,
                        Name = m.Recipient.Name
                    });

                    Assert.Single(participants);
                    Assert.Single(recipients);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        [Fact]
        public void ShouldBeAbleToGenerateGiftExchangeListWhenThereAreSelectiveMatches()
        {
            try
            {
                List<Member> members = new List<Member>
                {
                    new Member
                    {
                        Name = "Rick",
                        Id = new Guid("2cf0dfad-0f2f-4b54-913c-1146a706c1a6"),
                        SelectiveMatch = true
                    },

                    new Member
                    {
                        Name = "Reeba",
                        Id = new Guid("470cd67a-3383-4575-82bb-e067926afb0b")
                    },

                    new Member
                    {
                        Name = "Rhonda",
                        Id = new Guid("032f4c22-1d26-4a49-91f7-66bdb69588ed"),
                        SelectiveMatch = true
                    },
                    new Member
                    {
                        Name = "Ryan",
                        Id = new Guid("c0746e4b-38b5-4c57-b50a-2dc72611f712")
                    }
                };


                var exchangeListGen = new ExchangeListGenerator();

                var matches = exchangeListGen.Generate(members, null);

                //should be no errors
                var errors = matches.Where(m => !string.IsNullOrEmpty(m.ErrorMessage)).ToList();
                Assert.Empty(errors);


                foreach (var member in members)
                {
                    //each member should only appear once as participant and once as a recipient

                    var participantMatches = matches.Where(m => m.Giver.Id == member.Id);
                    var participants =  participantMatches.Select(m => new Member
                    {
                        Id = m.Giver.Id,
                        Name = m.Giver.Name,
                    });

                    var recipientMatches = matches.Where(m => m.Recipient.Id == member.Id);
                    var recipients = recipientMatches.Select(m => new Member
                    {
                        Id = m.Recipient.Id,
                        Name = m.Recipient.Name
                    });

                    Assert.Single(participants);
                    Assert.Single(recipients);

                    if (member.SelectiveMatch)
                    {
                        var recipient = participantMatches.FirstOrDefault().Recipient;
                        Assert.False(recipient.SelectiveMatch);
                    }


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }


        [Fact]
        public void ShouldGenerateErrorWhenThereAreMoreSelectedMatchMembersThanTotalParticipants()
        {
            try
            {
                List<Member> members = new List<Member>
                {
                    new Member
                    {
                        Name = "Rick",
                        Id = new Guid("2cf0dfad-0f2f-4b54-913c-1146a706c1a6"),
                        SelectiveMatch = true
                    },

                    new Member
                    {
                        Name = "Reeba",
                        Id = new Guid("470cd67a-3383-4575-82bb-e067926afb0b"),
                        SelectiveMatch = true
                    },

                    new Member
                    {
                        Name = "Rhonda",
                        Id = new Guid("032f4c22-1d26-4a49-91f7-66bdb69588ed"),
                        SelectiveMatch = true
                    },
                    new Member
                    {
                        Name = "Ryan",
                        Id = new Guid("c0746e4b-38b5-4c57-b50a-2dc72611f712")
                    }
                };


                var exchangeListGen = new ExchangeListGenerator();

                var matches = exchangeListGen.Generate(members, 1);

                //should be errors
                Assert.Contains(matches, m => m.ErrorMessage != string.Empty);


                foreach (var member in members)
                {
                    //each member should only appear once as participant and once as a recipient

                    var participantMatches = matches.Where(m => m.Giver.Id == member.Id);
                    var participants = participantMatches.Select(m => new Member
                    {
                        Id = m.Giver.Id,
                        Name = m.Giver.Name,
                    });


                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
    }


    
}
