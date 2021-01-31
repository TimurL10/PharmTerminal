using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication5.Models
{
    public class MarketNames
    {
        public MarketNames(Guid tableRowGUID, string namefull)
        {
            this.TableRowGUID = tableRowGUID;
            this.NameFull = namefull;
        }

        public MarketNames(int count, string nameFull)
        {            
            this.Count = count;
            this.NameFull = nameFull;
        }

        public MarketNames(int count, Guid tableRowGUID)
        {            
            this.Count = count;
            this.TableRowGUID = tableRowGUID;
        }


        public Guid TableRowGUID { get; set; }
        public string NameFull { get; set; }
        public int Count { get; set; }
    }
}
