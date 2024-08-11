using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIT
{
    public interface IUserActions
    {
        public void Add() { }
        public void Pull() { }
        public void Commit() { }
        public void Push() { }
    }
}
