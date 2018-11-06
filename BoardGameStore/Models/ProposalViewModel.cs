using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoardGameStore.Models
{
    public class ProposalViewModel
    {
        public InventoryItem ProposerItem { get; set; }
        public InventoryItem ProposeeItem { get; set; }
        public BoardGameHubUser Proposer { get; set; }
        public BoardGameHubUser Proposee { get; set; }
    }
}
