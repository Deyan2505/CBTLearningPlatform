using CbtLearningPlatform.Client.Curriculum;

namespace CbtLearningPlatform.Tests;

/// <summary>Course Completion Certificate eligibility — pure logic only. The exam-result-is-session-only
/// behavior (reload drops it, no localStorage, no history) is architectural (the score is never read
/// from or written to storage anywhere in the app) rather than something this pure function could ever
/// violate, so it is verified by code inspection + browser QA, not by a unit test here.</summary>
public sealed class CertificateEligibilityTests
{
    [Fact]
    public void IsEligible_FifteenOfFifteenWeeks_ExactlyMinimumScore_ReturnsTrue()
    {
        Assert.True(CertificateEligibility.IsEligible(completedWeeks: 15, routedWeeks: 15, currentExamScore: 75));
    }

    [Fact]
    public void IsEligible_FifteenOfFifteenWeeks_AboveMinimumScore_ReturnsTrue()
    {
        Assert.True(CertificateEligibility.IsEligible(completedWeeks: 15, routedWeeks: 15, currentExamScore: 90));
    }

    [Fact]
    public void IsEligible_FifteenOfFifteenWeeks_OneBelowMinimumScore_ReturnsFalse()
    {
        Assert.False(CertificateEligibility.IsEligible(completedWeeks: 15, routedWeeks: 15, currentExamScore: 74));
    }

    [Fact]
    public void IsEligible_FourteenOfFifteenWeeks_PerfectScore_ReturnsFalse()
    {
        Assert.False(CertificateEligibility.IsEligible(completedWeeks: 14, routedWeeks: 15, currentExamScore: 100));
    }

    [Fact]
    public void IsEligible_FifteenOfFifteenWeeks_NoExamResultYet_ReturnsFalse()
    {
        Assert.False(CertificateEligibility.IsEligible(completedWeeks: 15, routedWeeks: 15, currentExamScore: null));
    }

    [Fact]
    public void IsEligible_NoWeeksComplete_NoExamResult_ReturnsFalse()
    {
        Assert.False(CertificateEligibility.IsEligible(completedWeeks: 0, routedWeeks: 15, currentExamScore: null));
    }

    [Fact]
    public void IsEligible_ZeroScore_FifteenOfFifteenWeeks_ReturnsFalse()
    {
        Assert.False(CertificateEligibility.IsEligible(completedWeeks: 15, routedWeeks: 15, currentExamScore: 0));
    }

    [Fact]
    public void MinimumExamScore_IsSeventyFive()
    {
        Assert.Equal(75, CertificateEligibility.MinimumExamScore);
    }
}
