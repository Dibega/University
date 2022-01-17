using System.Collections.Generic;

public interface INode
{
    Point Point { set; get; }
    INode Root { set; get; }
    List<INode> Neigbours { set; get; }
}
