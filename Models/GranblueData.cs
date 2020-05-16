using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GBVSFrameBot.Models
{
    public class GranblueData
    {
        public int Id{get;set;}
        public string Character {get;set;}
        public string Name {get;set;}
        public string Input {get;set;}
        public string Damage{get;set;}
        public string Guard{get;set;}
        public string StartUp{get;set;}
        public string Active{get;set;}
        public string Recovery{get;set;}
        public string OnBlock{get;set;}
        public string OnHit{get;set;}
        public string Attribute{get;set;}
        public string Level{get;set;}
        public string Blockstun{get;set;}
        public string Hitstun{get;set;}
        public string Untech{get;set;}
        public string Hitstop{get;set;}
        public string Invul{get;set;}
        public string Description{get;set;}
        public string ImageUrl{get;set;}
        public string ThumbnailUrl{get;set;}
        public string DustloopCharacterUrl{get;set;}
    }
}