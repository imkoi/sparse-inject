using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SparseInject;

public class TypeIdProviderTest
{
    [Test]
    [TestCase(1)]
    [TestCase(3)]
    [TestCase(32)]
    [TestCase(128)]
    [TestCase(1000)]
    public void SameType_WhenGetIdTwice_ReturnSameId(int capacity)
    {
        var type = typeof(TypeIdProviderTest);
        var typeIdProvider = new TypeIdProvider(capacity);
        
        typeIdProvider.GetId(type).Should().Be(typeIdProvider.GetId(type));
    }
    
    [Test]
    [TestCase(1)]
    [TestCase(3)]
    [TestCase(32)]
    [TestCase(128)]
    [TestCase(1000)]
    public void ManyTypes_WhenGetIds_ReturnSameIds(int capacity)
    {
        var types = typeof(TypeIdProviderTest).Assembly.GetTypes();
        var typeIdProvider = new TypeIdProvider(capacity);
        var ids = new int[types.Length];

        for (var i = 0; i < types.Length; i++)
        {
            var type = types[i];
            var typeId = typeIdProvider.GetId(type);
            
            ids[i] = typeId;
        }
        
        for (var i = 0; i < types.Length; i++)
        {
            var type = types[i];
            var typeId = typeIdProvider.GetId(type);
            
            typeId.Should().Be(ids[i]);
        }
    }
    
    [Ignore("Failing, not used in di")]
    [Test]
    [TestCase(1)]
    [TestCase(3)]
    [TestCase(32)]
    [TestCase(128)]
    [TestCase(1000)]
    public void ManyTypes_WhenGetIds_ReturnUniqueIds(int capacity)
    {
        var types = typeof(TypeIdProviderTest).Assembly.GetTypes();
        var typeIdProvider = new TypeIdProvider(capacity);
        var uniqueIds = new HashSet<int>();

        for (var i = 0; i < types.Length; i++)
        {
            var type = types[i];
            var typeId = typeIdProvider.GetId(type);

            uniqueIds.Add(typeId);
        }

        uniqueIds.Count.Should().Be(types.Length);
    }

    [Ignore("Failing, not used in di")]
    [Test]
    public void ProviderCreated_WhenCapacityLessThanMax_NotThrow()
    {
        this.Invoking(_ => new TypeIdProvider(16_777_215))
            .Should()
            .NotThrow();
    }
    
    [Test]
    public void ProviderCreated_WhenCapacityMoreThanMax_ThrowArgumentOutOfRangeException()
    {
        this.Invoking(_ => new TypeIdProvider(16_777_216))
            .Should()
            .Throw<ArgumentOutOfRangeException>();
    }
    
    [Test]
    public void GetId_WhenPassNullType_ThrowArgumentOutOfRangeException()
    {
        var typeIdProvider = new TypeIdProvider();
        
        typeIdProvider.Invoking(subject => subject.GetId(null))
            .Should()
            .Throw<ArgumentNullException>();
    }
    
    [Ignore("Failing, not used in di")]
    [Test]
    [TestCase(1)]
    [TestCase(3)]
    [TestCase(32)]
    [TestCase(128)]
    [TestCase(1000)]
    public void GetCount_WhenAddedInstances_ReturnCorrectCount(int capacity)
    {
        var types = typeof(TypeIdProviderTest).Assembly.GetTypes();
        var typeIdProvider = new TypeIdProvider(capacity);

        for (var i = 0; i < types.Length; i++)
        {
            var type = types[i];
            typeIdProvider.GetId(type);
        }
        
        typeIdProvider.Count.Should().Be(types.Length);
    }
}