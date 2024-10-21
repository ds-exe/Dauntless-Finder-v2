using Dauntless_Finder_v2.DataHandler.src.Scripts;
using NUnit.Framework;

namespace Dauntless_Finder_v2.Test.Tests;

public class GenerateIndexedDataShould
{
    public GenerateIndexedDataShould()
    {

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
    public void GenerateArmourData()
    {
        var sut = GenerateIndexedData.GenerateArmourData();

        Assert.That(sut, Is.Not.Null);
        Assert.That(sut.Heads.ContainsKey(6), Is.True);
        Assert.That(sut.Heads[6].ContainsKey(34), Is.True);
        Assert.That(sut.Heads[6][34].ContainsKey(72), Is.True);
        Assert.That(sut.Heads[6][34][72].FirstOrDefault(), Is.Not.Null);
        Assert.That(sut.Heads[6][34][72].FirstOrDefault().Id, Is.EqualTo(1));
        Assert.That(sut.Heads[6][34][72].FirstOrDefault().Perks.ContainsKey(6), Is.True);
        Assert.That(sut.Heads[6][34][72].FirstOrDefault().Perks[6], Is.EqualTo(2));

        Assert.That(sut.Heads.ContainsKey(31), Is.True);
        Assert.That(sut.Heads[31].ContainsKey(54), Is.True);
        Assert.That(sut.Heads[31][54].ContainsKey(76), Is.True);
        Assert.That(sut.Heads[31][54][76].FirstOrDefault(), Is.Not.Null);
        Assert.That(sut.Heads[31][54][76].FirstOrDefault().Id, Is.EqualTo(5));
        Assert.That(sut.Heads[31][54][76].FirstOrDefault().Perks.ContainsKey(31), Is.True);
        Assert.That(sut.Heads[31][54][76].FirstOrDefault().Perks[31], Is.EqualTo(3));
    }
}
