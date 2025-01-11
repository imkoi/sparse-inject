public class Dependency_Depth2
{
    private readonly DependencyD1_Depth2 _d1;
    private readonly DependencyD2_Depth2 _d2;

    public Dependency_Depth2(DependencyD1_Depth2 d1, DependencyD2_Depth2 d2)
    {
        _d1 = d1;
        _d2 = d2;
    }
}

public class DependencyD1_Depth2
{
    public DependencyD1_Depth2() { }
}

public class DependencyD2_Depth2
{
    public DependencyD2_Depth2() { }
}


