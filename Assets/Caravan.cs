using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets
{
    public class Caravan
    {
        public Character Leader { get { return Followers[0]; }}
        public GameObject Model { get; set; }
        public List<Character> Followers { get; set; }

        public Caravan()
        {
            Model = new GameObject("caravan");
            Followers = new List<Character>();
            Followers.Add(new Character(Model.transform)); //leader
            
            for (int i = 1; i < 20; i++)
            {
                Followers.Add(new Character(Model.transform));
            }
        }
    }
}
