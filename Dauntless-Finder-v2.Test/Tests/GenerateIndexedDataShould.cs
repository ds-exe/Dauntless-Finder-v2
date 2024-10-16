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
        Assert.That(sut.Helms.ContainsKey(6), Is.True);
        Assert.That(sut.Helms[6].ContainsKey(34), Is.True);
        Assert.That(sut.Helms[6][34].ContainsKey(72), Is.True);
        Assert.That(sut.Helms[6][34][72].FirstOrDefault(), Is.Not.Null);
        Assert.That(sut.Helms[6][34][72].FirstOrDefault().Perks.ContainsKey(6), Is.True);
        Assert.That(sut.Helms[6][34][72].FirstOrDefault().Perks[6], Is.EqualTo(2));
    }
}
