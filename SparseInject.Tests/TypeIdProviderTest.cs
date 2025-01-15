using System;
using System.Collections.Generic;
using System.Reflection;
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

        typeIdProvider.TryAdd(type, out var value1);
        typeIdProvider.TryAdd(type, out var value2);

        value1.Should().Be(value2);
    }
    
    [Test]
    [TestCase(0)]
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
            typeIdProvider.TryAdd(type, out var typeId);
            
            ids[i] = typeId;
        }
        
        for (var i = 0; i < types.Length; i++)
        {
            var type = types[i];
            typeIdProvider.TryAdd(type, out var typeId);
            
            typeId.Should().Be(ids[i]);
        }
    }
    
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
            typeIdProvider.TryAdd(type, out var typeId);

            uniqueIds.Add(typeId);
        }

        uniqueIds.Count.Should().Be(types.Length);
    }
    
    [Test]
    [TestCase(1)]
    [TestCase(3)]
    [TestCase(32)]
    [TestCase(128)]
    [TestCase(1000)]
    public void ManyTypes_TryGetIds_ReturnUniqueIds(int capacity)
    {
        var types = typeof(TypeIdProviderTest).Assembly.GetTypes();
        var typeIdProvider = new TypeIdProvider(capacity);
        var uniqueIds = new HashSet<int>();

        for (var i = 0; i < types.Length; i++)
        {
            var type = types[i];
            typeIdProvider.TryAdd(type, out var typeId);

            uniqueIds.Add(typeId);
        }

        uniqueIds.Count.Should().Be(types.Length);

        for (int i = 0; i < types.Length; i++)
        {
            var type = types[i];
            typeIdProvider.TryGetValue(type, out var typeId);

            uniqueIds.Contains(typeId).Should().BeTrue();
        }
    }
    
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
        
        typeIdProvider.Invoking(subject => subject.TryAdd(null, out var _))
            .Should()
            .Throw<ArgumentNullException>();
    }
    
    [Test]
    public void TryGetId_WhenPassNullType_ThrowArgumentOutOfRangeException()
    {
        var typeIdProvider = new TypeIdProvider();
        
        typeIdProvider.Invoking(subject => subject.TryGetValue(null, out _))
            .Should()
            .Throw<ArgumentNullException>();
    }
    
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
            typeIdProvider.TryAdd(type, out _);
        }
        
        typeIdProvider.Count.Should().Be(types.Length);
    }
}