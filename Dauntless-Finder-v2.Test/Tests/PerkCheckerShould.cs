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
            Assert.That(sut.Contains(i), Is.False);
        }
    }

    [Test]
    public void RequestAll()
    {
        List<int> requiredPerks = [];
        List<int> requestedPerks = [];
        for (int i = 1; i <= 80; i++)
        {
            requestedPerks.Add(i);
        }
        var sut = perkChecker.GetAvailablePerks(requiredPerks.ToList(), requestedPerks.ToList());

        foreach (int i in requestedPerks)
        {
            Assert.That(sut.Contains(i), Is.True);
        }
    }

    [Test]
    public void RequireAllThreshold2()
    {
        List<int> requiredPerks = [5, 8, 17, 24, 30, 44, 48, 71];
        List<int> requestedPerks = [77];
        var sut = perkChecker.GetAvailablePerks(requiredPerks.ToList(), requestedPerks.ToList());

        foreach (int i in requestedPerks)
        {
            Assert.That(sut.Contains(i), Is.False);
        }
    }

    [Test]
    public void RequireAllThreshold3()
    {
        List<int> requiredPerks = [7, 10, 13, 22, 25, 42, 72];
        List<int> requestedPerks = [76];
        var sut = perkChecker.GetAvailablePerks(requiredPerks.ToList(), requestedPerks.ToList());

        foreach (int i in requestedPerks)
        {
            Assert.That(sut.Contains(i), Is.False);
        }
    }

    [Test]
    public void RequireAllThreshold6()
    {
        List<int> requiredPerks = [4, 6, 11, 12, 18, 19, 21, 27, 33, 36, 49, 51, 53, 54, 56, 58];
        List<int> requestedPerks = [67];
        var sut = perkChecker.GetAvailablePerks(requiredPerks.ToList(), requestedPerks.ToList());

        foreach (int i in requestedPerks)
        {
            Assert.That(sut.Contains(i), Is.False);
        }
    }

    [Test]
    public void RequireAllThreshold7()
    {
        List<int> requiredPerks = [2, 40, 59, 60, 65];
        List<int> requestedPerks = [79];
        var sut = perkChecker.GetAvailablePerks(requiredPerks.ToList(), requestedPerks.ToList());

        foreach (int i in requestedPerks)
        {
            Assert.That(sut.Contains(i), Is.False);
        }
    }

    [Test]
    public void RequireAllThreshold8()
    {
        List<int> requiredPerks = [16, 32];
        List<int> requestedPerks = [57];
        var sut = perkChecker.GetAvailablePerks(requiredPerks.ToList(), requestedPerks.ToList());

        foreach (int i in requestedPerks)
        {
            Assert.That(sut.Contains(i), Is.False);
        }
    }

    [Test]
    public void HighPerkTests()
    {
        var sut = perkChecker.GetAvailablePerks([16], [32]);

        Assert.That(sut.Contains(32), Is.True);

        var sut2 = perkChecker.GetAvailablePerks([57], [16]);

        Assert.That(sut2.Contains(16), Is.True);

        var sut3 = perkChecker.GetAvailablePerks([32], [57]);

        Assert.That(sut3.Contains(57), Is.True);
    }

    [Test]
    public void LimitedBuildTest()
    {
        List<int> requestedPerks = [8, 20, 26, 32, 38, 41, 42, 46, 48, 50, 51, 55, 67];
        var sut = perkChecker.GetAvailablePerks([32, 57], requestedPerks.ToList());

        foreach (int i in requestedPerks)
        {
            Assert.That(sut.Contains(i), Is.True);
        }
    }

    [Test]
    public void LimitedBuildTestReverse()
    {
        List<int> requestedPerks = [];
        for (int i = 1; i <= 80; i++)
        {
            requestedPerks.Add(i);
        }
        List<int> removedPerks = [8, 20, 26, 32, 38, 41, 42, 46, 48, 50, 51, 55, 67, 32, 57];
        requestedPerks = requestedPerks.Except(removedPerks).ToList();

        var sut = perkChecker.GetAvailablePerks([32, 57], requestedPerks.ToList());

        foreach (int i in requestedPerks)
        {
            Assert.That(sut.Contains(i), Is.False);
        }
    }
}
