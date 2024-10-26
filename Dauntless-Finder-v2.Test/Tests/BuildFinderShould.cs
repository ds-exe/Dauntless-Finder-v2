using Dauntless_Finder_v2.App.src.Scripts;
using NUnit.Framework;

namespace Dauntless_Finder_v2.Test.Tests;

public class BuildFinderShould
{
    protected BuildFinder buildFinder { get; set; }

    public BuildFinderShould()
    {

    }

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        buildFinder = new BuildFinder();
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
        var sut = buildFinder.GetBuilds([5, 8, 17, 24, 30, 36], 50);

        Assert.That(sut, Is.Not.Null);
        Assert.That(sut.Count, Is.EqualTo(47));
    }

    [Test]
    public void Test2()
    {
        List<int> requiredPerks = [];
        List<int> requestedPerks = new() { 1 };
        var sut = buildFinder.GetBuilds([5, 8, 17, 24, 30, 36], 20);

        Assert.That(sut, Is.Not.Null);
        Assert.That(sut.Count, Is.EqualTo(20));
    }
}
