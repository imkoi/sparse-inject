public static class ManualResolver_Depth3
{
    public static Dependency_Depth3 CreateDependency()
    {
        return new Dependency_Depth3(CreateDependencyD1(), CreateDependencyD2());
    }

    public static DependencyD1_Depth3 CreateDependencyD1()
    {
        return new DependencyD1_Depth3(CreateDependencyD1D1(), CreateDependencyD1D2(), CreateDependencyD1D3(), CreateDependencyD1D4(), CreateDependencyD1D5());
    }

    public static DependencyD1D1_Depth3 CreateDependencyD1D1()
    {
        return new DependencyD1D1_Depth3();
    }

    public static DependencyD1D2_Depth3 CreateDependencyD1D2()
    {
        return new DependencyD1D2_Depth3();
    }

    public static DependencyD1D3_Depth3 CreateDependencyD1D3()
    {
        return new DependencyD1D3_Depth3();
    }

    public static DependencyD1D4_Depth3 CreateDependencyD1D4()
    {
        return new DependencyD1D4_Depth3();
    }

    public static DependencyD1D5_Depth3 CreateDependencyD1D5()
    {
        return new DependencyD1D5_Depth3();
    }

    public static DependencyD2_Depth3 CreateDependencyD2()
    {
        return new DependencyD2_Depth3(CreateDependencyD2D1(), CreateDependencyD2D2(), CreateDependencyD2D3(), CreateDependencyD2D4(), CreateDependencyD2D5());
    }

    public static DependencyD2D1_Depth3 CreateDependencyD2D1()
    {
        return new DependencyD2D1_Depth3();
    }

    public static DependencyD2D2_Depth3 CreateDependencyD2D2()
    {
        return new DependencyD2D2_Depth3();
    }

    public static DependencyD2D3_Depth3 CreateDependencyD2D3()
    {
        return new DependencyD2D3_Depth3();
    }

    public static DependencyD2D4_Depth3 CreateDependencyD2D4()
    {
        return new DependencyD2D4_Depth3();
    }

    public static DependencyD2D5_Depth3 CreateDependencyD2D5()
    {
        return new DependencyD2D5_Depth3();
    }

}
