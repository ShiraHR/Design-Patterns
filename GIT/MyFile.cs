using GIT.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace GIT;

public class MyFile : Component
{
    public MyFile(string name, string owner, Component container, string content = null) : base(name, owner)
    {
        history = new();
        this.Content = content;
        this.container = container;
    }
    private Component container;

    private Stack<MyFile> history;

    public string Content { get; private set; }
    #region getFunctions
    public override Component GetParent()
    {
        return this.container;
    }
    public override MyFile GetFile(string name)
    {
        if(this.Name== name) return this;
        return null;
    }
    public override Folder GetFolder(string name)
    {
        return null;
    }
    #endregion
    #region otherFunctions
    public override Component Clone(string name)
    {
        return new MyFile(Name, this.Owner, this.container, this.Content)
        {
            Status = Draft.GetInstance()
        };
    }
    public void SetContent(string content)
    {
        history.Push(new MyFile(this.Name, this.Owner, this.container) { Status = this.Status, Content = this.Content });
        this.Content = content;
        this.Status = Draft.GetInstance();
    }
    public override void Merge(Component other)
    {
        if (this.Status.GetStatus() != "ReadyToMerge")
        {
            Console.WriteLine("you can not merge");
            return;
        }
        this.Content = ((MyFile)other).Content;
        Status.ChangeStatus(this);
    }
    public override void Undo()
    {
        try
        {
            MyFile last = history.Pop();
            if (last == null) return;
            this.Status = last.Status;
            this.Content = last.Content;
        }
        catch
        {
            Console.WriteLine($"nothing to undo in {this.Name}");
        }
    }
    public override void Print(int num = 0)
    {
        for (int i = 0; i < num; i++)
        {

            Console.Write('\t');
        }
        Console.Write($"I'm file - {this.Name} and my content is : {this.Content}\n");
    }
    #endregion
}
