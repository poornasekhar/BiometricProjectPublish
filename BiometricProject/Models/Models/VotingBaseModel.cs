using BiometricProject.Models.DataBaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiometricProject.Models.Models
{
    public class VotingBaseModel
    {
        public UserDetails userDetails { get; set; }
        public List<PartyDetails> partyDetails { get; set; }
    }
}