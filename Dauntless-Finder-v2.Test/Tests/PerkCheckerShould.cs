using BenchmarkDotNet.Attributes;
using Dauntless_Finder_v2.App.src.Scripts;
using NUnit.Framework;

namespace Dauntless_Finder_v2.Test.Tests;

public class PerkCheckerShould
{
    PerkChecker perkChecker = new PerkChecker();

    public PerkCheckerShould()
    {

    }

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        perkChecker = new PerkChecker();
    }

    [SetUp]
    public void SetUp()
    {

    }

    [TearDown]
    public void TearDown()
    {

    }

    [Test]
    public void Test1()
    {
        List<int> requiredPerks = [];
        List<int> requestedPerks = new() { 1 };
        var sut = perkChecker.GetAvailablePerks(requiredPerks.ToList(), requestedPerks.ToList());

        Assert.That(sut.Contains(1), Is.True);
    }

    [Test]
    public void Test2()
    {
        List<int> requiredPerks = [];
        List<int> requestedPerks = new() { 1, 6, 3, 5 };
        var sut = perkChecker.GetAvailablePerks(requiredPerks.ToList(), requestedPerks.ToList());

        foreach (int i in requestedPerks)
        {
            Assert.That(sut.Contains(i), Is.True);
        }
    }

    [Test]
    public void Test3()
    {
        List<int> requiredPerks = new() { 1, 6, 3, 5 };
        List<int> requestedPerks = new() { 4 };
        var sut = perkChecker.GetAvailablePerks(requiredPerks.ToList(), requestedPerks.ToList());

        foreach (int i in requestedPerks)
        {
            Assert.That(sut.Contains(i), Is.True);
        }
    }
}
