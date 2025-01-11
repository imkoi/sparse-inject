public class Dependency_Depth3
{
    private readonly DependencyD1_Depth3 _d1;
    private readonly DependencyD2_Depth3 _d2;

    public Dependency_Depth3(DependencyD1_Depth3 d1, DependencyD2_Depth3 d2)
    {
        _d1 = d1;
        _d2 = d2;
    }
}

public class DependencyD1_Depth3
{
    private readonly DependencyD1D1_Depth3 _d1;
    private readonly DependencyD1D2_Depth3 _d2;
    private readonly DependencyD1D3_Depth3 _d3;
    private readonly DependencyD1D4_Depth3 _d4;
    private readonly DependencyD1D5_Depth3 _d5;

    public DependencyD1_Depth3(DependencyD1D1_Depth3 d1, DependencyD1D2_Depth3 d2, DependencyD1D3_Depth3 d3, DependencyD1D4_Depth3 d4, DependencyD1D5_Depth3 d5)
    {
        _d1 = d1;
        _d2 = d2;
        _d3 = d3;
        _d4 = d4;
        _d5 = d5;
    }
}

public class DependencyD1D1_Depth3
{
    public DependencyD1D1_Depth3() { }
}

public class DependencyD1D2_Depth3
{
    public DependencyD1D2_Depth3() { }
}

public class DependencyD1D3_Depth3
{
    public DependencyD1D3_Depth3() { }
}

public class DependencyD1D4_Depth3
{
    public DependencyD1D4_Depth3() { }
}

public class DependencyD1D5_Depth3
{
    public DependencyD1D5_Depth3() { }
}

public class DependencyD2_Depth3
{
    private readonly DependencyD2D1_Depth3 _d1;
    private readonly DependencyD2D2_Depth3 _d2;
    private readonly DependencyD2D3_Depth3 _d3;
    private readonly DependencyD2D4_Depth3 _d4;
    private readonly DependencyD2D5_Depth3 _d5;

    public DependencyD2_Depth3(DependencyD2D1_Depth3 d1, DependencyD2D2_Depth3 d2, DependencyD2D3_Depth3 d3, DependencyD2D4_Depth3 d4, DependencyD2D5_Depth3 d5)
    {
        _d1 = d1;
        _d2 = d2;
        _d3 = d3;
        _d4 = d4;
        _d5 = d5;
    }
}

public class DependencyD2D1_Depth3
{
    public DependencyD2D1_Depth3() { }
}

public class DependencyD2D2_Depth3
{
    public DependencyD2D2_Depth3() { }
}

public class DependencyD2D3_Depth3
{
    public DependencyD2D3_Depth3() { }
}

public class DependencyD2D4_Depth3
{
    public DependencyD2D4_Depth3() { }
}

public class DependencyD2D5_Depth3
{
    public DependencyD2D5_Depth3() { }
}


