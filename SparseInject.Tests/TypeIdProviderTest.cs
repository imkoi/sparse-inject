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
        
        typeIdProvider.GetOrAddId(type).Should().Be(typeIdProvider.GetOrAddId(type));
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
            var typeId = typeIdProvider.GetOrAddId(type);
            
            ids[i] = typeId;
        }
        
        for (var i = 0; i < types.Length; i++)
        {
            var type = types[i];
            var typeId = typeIdProvider.GetOrAddId(type);
            
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
            var typeId = typeIdProvider.GetOrAddId(type);

            uniqueIds.Add(typeId);
        }

        uniqueIds.Count.Should().Be(types.Length);
    }
    
    // [Test]
    // public void ManyTypeGetIds_WhenExceedCapacity_ThrowIndexOutOfRangeException()
    // {
    //     var types = typeof(TypeIdProviderTest).Assembly.GetTypes();
    //     var typeIdProvider = new TypeIdProvider(1);
    //
    //     var oldPrimes = TypeIdProvider._primes;
    //
    //     TypeIdProvider._primes = new int[] { 2, 3, 5, 7, 11 };
    //
    //     try
    //     {
    //         this.Invoking(_ =>
    //             {
    //                 for (var i = 0; i < types.Length; i++)
    //                 {
    //                     var type = types[i];
    //                     typeIdProvider.GetOrAddId(type);
    //                 }
    //             })
    //             .Should()
    //             .Throw<IndexOutOfRangeException>();
    //     }
    //     finally
    //     {
    //         TypeIdProvider._primes = oldPrimes;
    //     }
    // }
    
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
            var typeId = typeIdProvider.GetOrAddId(type);

            uniqueIds.Add(typeId);
        }

        uniqueIds.Count.Should().Be(types.Length);

        for (int i = 0; i < types.Length; i++)
        {
            var type = types[i];
            typeIdProvider.TryGetId(type, out var typeId);

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
        
        typeIdProvider.Invoking(subject => subject.GetOrAddId(null))
            .Should()
            .Throw<ArgumentNullException>();
    }
    
    [Test]
    public void TryGetId_WhenPassNullType_ThrowArgumentOutOfRangeException()
    {
        var typeIdProvider = new TypeIdProvider();
        
        typeIdProvider.Invoking(subject => subject.TryGetId(null, out _))
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
            typeIdProvider.GetOrAddId(type);
        }
        
        typeIdProvider.Count.Should().Be(types.Length);
    }
}