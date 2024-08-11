using GIT.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIT;

public class Folder : Component
{
    private Stack<Folder> history;
    private Component container;
    public Dictionary<string, Component> Children { get; set; }
    public Folder(string name, string owner, Component container) : base(name, owner)
    {
        Children = new();
        history = new();
        this.container = container;
    }
    #region getFunctions
    public override Component GetParent()
    {
        try
        {
            return (Branch)this.container;
        }
        catch
        {
            return this.GetParent();
        }
    }
    public override Folder GetFolder(string name)
    {
        if (this.Name == name) return this;
        Folder res = null;
        foreach (var branch in Children)
        {
            try
            {
                res = ((Folder)branch.Value).GetFolder(name);
            }
            catch
            {
            }
            if (res != null)
            {
                return res;
            }
        }

        return null;
    }
    public override MyFile GetFile(string name)
    {
        MyFile res;
        foreach (var item in Children)
        {
            res = item.Value.GetFile(name);
            if (res != null)
            {
                return res;
            }
        }
        return null;
    }
    #endregion
    #region addFunctions
    private void Add(Component component)
    {
        if (component.GetType() == typeof(Branch)) return;
        Children.Add(component.Name, component);
        Status = Draft.GetInstance();
    }
    public void AddFile(string name, string content = null)
    {
        this.Add(new MyFile(name, this.Owner, this, content));
    }
    public void AddFolder(string name)
    {
        this.Add(new Folder(name, this.Owner, this));
    }
    #endregion
    #region otherFunctions
    public override Component Clone(string name)
    {
        Folder folder = new(Name, Owner, container)
        {
            Status = Status,
            Children = new()
        };
        foreach (var child in Children)
        {
            folder.Children.Add(child.Key, child.Value.Clone(child.Value.Name));
        }
        folder.Status = Draft.GetInstance();
        return folder;
    }
    public override void Merge(Component other)
    {
        if (this.Status.GetStatus() != "ReadyToMerge")
        {
            Console.WriteLine("you can not merge");
            return;
        }
        history.Push(new Folder(this.Name, this.Owner, this.container) { Status = this.Status, Children = this.Children });
        foreach (var child in ((Folder)other).Children)
        {
            try
            {
                this.Children[child.Key].Merge(child.Value);
            }
            catch (Exception)
            {
                this.Children.Add(child.Key, child.Value);
            }
        }
        Status.ChangeStatus(this);
    }
    public override void Undo()
    {
        try
        {
            Folder last = history.Pop();
            if (last == null) return;
            this.Status = last.Status;
            this.Children = last.Children;
        }
        catch
        {
            Console.WriteLine($"nothing to undo in {this.Name}");

        }
        foreach (var child in this.Children)
        {
            child.Value.Undo();
        }
    }
    public override void Print(int num = 0)
    {
        for (int i = 0; i < num; i++)
        {
            Console.Write('\t');
        }
        Console.WriteLine($"I'm folder {this.Name} and my children are:");
        foreach (var child in Children)
        {
            child.Value.Print(num + 1);
        }
    }
    #endregion
}
