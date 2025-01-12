public static class ManualResolver_Depth2
{
    public static Dependency_Depth2 CreateDependency()
    {
        return new Dependency_Depth2(CreateDependencyD1(), CreateDependencyD2());
    }

    public static DependencyD1_Depth2 CreateDependencyD1()
    {
        return new DependencyD1_Depth2();
    }

    public static DependencyD2_Depth2 CreateDependencyD2()
    {
        return new DependencyD2_Depth2();
    }

}
